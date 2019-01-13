using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Ecalpon 
{
	/// <summary>
	/// Holds member variables and functions used to manage a conflict
	/// </summary>
	public class CCombat
	{
		public int[,] FightBase = new int[19,19];
		public int[,] FightOverlay = new int[19,19];
		
		public int FightRow;
		public int FightCol;

		private int BodyCount=0;

		public enum Locale {OUTSIDE, ENCOUNTER};
		public enum Victim {MONSTER, CHARACTER};

		private Locale eFightLocale;
		public CLoot oLoot;
		private CMonsters oMonsters;

		/// <summary>
		/// Moves the turn
		/// </summary>
		public void NextChar() 
		{
			CEcalpon.FightCharMoveOrder++;

			if(CEcalpon.FightCharMoveOrder > CEcalpon.gobjParty.Count) 
			{
				if(!CEcalpon.TimeSlip) 
				{
					CEcalpon.FightMonstMove = true;
					CEcalpon.FightCharMove = false;
				}
				else 
				{
					CEcalpon.TimeSlip = false;
					CEcalpon.FightCharMoveOrder = CEcalpon.gobjParty.FindNextConsciousChar(1);
					if(CEcalpon.FightCharMoveOrder==0) 
					{
						CEcalpon.FightMonstMove = true;
						CEcalpon.FightCharMove = false;
					}
				}
			}
			else 
			{
				if(CEcalpon.gobjParty.GetCharacterByID(CEcalpon.gobjParty.GetCharacterIDByOrder(CEcalpon.FightCharMoveOrder)).HitPoints <= 0)
				{
					//Char is dead or unconscious...skip to the next one
					CEcalpon.FightCharMoveOrder = CEcalpon.gobjParty.FindNextConsciousChar(CEcalpon.FightCharMoveOrder);
					if(CEcalpon.FightCharMoveOrder==0)
						//5th character is dead
					{
						CEcalpon.FightMonstMove = true;
						CEcalpon.FightCharMove = false;
					}
				}
			}

		}

		/// <summary>
		/// Attack spell casting.
		/// </summary>
		public void ProcessSpellAttack()
		{
			int
				iDamage = 0;

			CSpell oSpell = new CSpell();
			oSpell.LoadByID(CEcalpon.SpellID);

			CCharacter oCharacter = CEcalpon.gobjParty.GetCharacterByOrder(CEcalpon.FightCharMoveOrder);

			switch(oSpell.ID) 
			{
				case 1: //magic missle
					iDamage = CEcalpon.MyRand(1, oSpell.Modifier) * oCharacter.Level;
					ValidateSpellAttack(CEcalpon.SpellParm, iDamage);
					break;
				case 3: //fireball!!
					//GroupHitSpell();
					break;
			}

			CEcalpon.SpellPending = false;

			NextChar();
		}


		/// <summary>
		/// Validate the target of the spell during an attack
		/// </summary>
		/// <param name="Direction"></param>
		/// <param name="Damage"></param>
		public void ValidateSpellAttack(string Direction, int Damage) 
		{
			int 
				sRow = 0,
				sCol = 0;
			
			CCharacter oCharacter = CEcalpon.gobjParty.GetCharacterByOrder(CEcalpon.FightCharMoveOrder);

			switch(Direction)
			{
				case "N":
					for(int dRow=sRow; dRow>-1; dRow--)
						if((FightOverlay[dRow, sCol] < 60) && (FightOverlay[dRow, sCol] > 0))
						{
							SpellHitMonster(dRow, sCol, Damage);
							break;
						}
					break;
				case "S":
					for(int dRow=sRow; dRow<19; dRow++)
						if((FightOverlay[dRow, sCol] < 60) && (FightOverlay[dRow, sCol] > 0))
						{
							SpellHitMonster(dRow, sCol, Damage);
							break;
						}
					break;
				case "E":
					for(int dCol=sCol; dCol<19; dCol++)
						if((FightOverlay[sRow, dCol] < 60) && (FightOverlay[sRow, dCol] > 0))
						{
							SpellHitMonster(sRow, dCol, Damage);
							break;
						}
					break;
				case "W":
					for(int dCol=sCol; dCol>-1; dCol--)
						if((FightOverlay[sRow, dCol] < 60) && (FightOverlay[sRow, dCol] > 0))
						{
							SpellHitMonster(sRow, dCol, Damage);
							break;
						}
					break;
			}

			CEcalpon.ShowMessage("Damage: " + Damage.ToString(), 1);

		}

		/// <summary>
		/// Determine if the monster was hit by a spell cast cardinally
		/// </summary>
		/// <param name="r"></param>
		/// <param name="c"></param>
		/// <param name="damage"></param>
		public void SpellHitMonster(int r, int c, int damage) 
		{	
			CCharacter oCharacter = CEcalpon.gobjParty.GetCharacterByOrder(CEcalpon.FightCharMoveOrder);

			foreach(CMonster oMonster in oMonsters) 
			{
				if(oMonster.FightRow==r && oMonster.FightCol==c)
				{
					if(oMonster.HitPoints>0)
					{
						oMonster.HitPoints -= damage;
						CEcalpon.ShowMessage("Damage: " + damage.ToString(), 1);
						if(oMonster.HitPoints<=0) 
						{
							oCharacter.Experience += oMonster.ExperienceValue;
							FightOverlay[oMonster.FightRow, oMonster.FightCol] = 0;
							BodyCount++;
						}
						CheckBattleStatus();
						break;
					}//check for hitpoints > 0
				}//check for correct monster
			}//find monster
		}


		/// <summary>
		/// Compute the damage of whomever is hit.
		/// </summary>
		/// <param name="idx"></param>
		/// <param name="Assailant"></param>
		/// <returns></returns>
		public int ComputeDamage(int idx, Victim Assailant) 
		{
			int
				mDamMin = 0,
				mDamMax = 0,
				mModifier = 0,
				iDamage = 0;
			bool
				bFizzle = false;
			
			CMonster oMonster=null;
			CCharacter oCharacter=null;
			
			switch(Assailant) 
			{
				case Victim.MONSTER:
					oMonster = oMonsters.Item(idx);
					mModifier = oMonster.DamageModifier;
					mDamMin = oMonster.Weapon.DamageMin;
					mDamMax = oMonster.Weapon.DamageMax;
					break;
				case Victim.CHARACTER:
					oCharacter = CEcalpon.gobjParty.GetCharacterByInternalIndex(idx);
					mModifier = oCharacter.Weapon.Modifier;
					mDamMin = oCharacter.Weapon.DamageMin;
					mDamMax = oCharacter.Weapon.DamageMax;

					//If this is a charged weapon (i.e. magic missle wand) reduce
					//charges
					if(oCharacter.Weapon.Charges == 0) 
					{
						oCharacter.Inventory.Remove(oCharacter.Weapon.InternalIndex);
						bFizzle = true;
						oCharacter.WeaponID = 99;
						CEcalpon.ShowMessage("Fizzle!", 5);
					}
					if(oCharacter.Weapon.Charges > 0)
						oCharacter.Weapon.Charges--;

					//Fighter's get a strength advantage
					if(oCharacter.ClassID == (int)CCharacter.eClass.Fighter) 
						switch(oCharacter.Strength) 
						{
							case 25: case 24: case 23: case 22: case 21:
								mModifier += CEcalpon.MyRand(6,(oCharacter.Strength - 10 + 3));
								break;
							case 20:
								mModifier += CEcalpon.MyRand(4, 12);
								break;
							case 19:
								mModifier += CEcalpon.MyRand(2, 10);
								break;
							case 18:
								mModifier += CEcalpon.MyRand(8);
								break;
							case 17:
								mModifier += CEcalpon.MyRand(6);
								break;
							case 16:
								mModifier += CEcalpon.MyRand(4);
								break;
						}
					break;
			}

			//compute and return damage
			if((mDamMax > 0) && (!bFizzle))
				iDamage = CEcalpon.MyRand(mDamMin, mDamMax) + mModifier;
			else
				iDamage = 0;

			return iDamage;

		}

		/// <summary>
		/// Move the monsters on the fight overlay
		/// </summary>
		public void FightMoveMonsters() 
		{
			CMonster oTargetMonster = null;
			CCharacter oCharacter = null;

			int 
				iDamage = 0,
				mRow = 0,
				mCol = 0,
				VRow = 0,
				VCol = 0;
			bool
				bFound = false;

			foreach(CMonster oMonster in oMonsters) 
			{
				if(oMonster.HitPoints > 0) 
				{
					oTargetMonster = oMonster;
					if(!TargetAcquisition(oTargetMonster, out VRow, out VCol))
					{
						//Find the row modifier
						bFound = false;
						for(int rscan=0; rscan<19; rscan++) 
						{
							for(int cscan=0; cscan<19; cscan++) 
							{
								if(FightOverlay[rscan,cscan] >= 80) 
								{
									bFound = true;
									mCol = cscan;
									mRow = rscan;
									break;
								}
							}
							if(bFound)
								break;
						}//scan for a target
						//move the monster
						ValidateMonsterMove(oTargetMonster, mRow, mCol);
					} //check target aquisition
					else //we are adjacent to the character...go ahead and attack
					{
						oCharacter = CEcalpon.gobjParty.FightFindCharacter(VRow, VCol);
						if(GotHit(oTargetMonster, oCharacter, Victim.CHARACTER)) 
						{
							iDamage = ComputeDamage(oMonster.InternalIndex, Victim.MONSTER);
							oCharacter.HitPoints -= iDamage;
							if(oCharacter.HitPoints<=0) 
							{
								//remove character from combat
								FightOverlay[oCharacter.FightRow, oCharacter.FightCol] = 0;
								CEcalpon.ShowMessage(oCharacter.Name + " Died!!!", 5);
								CEcalpon.PlaySound(CEcalpon.Sounds.DEATH);
							}
							else
							{
								CEcalpon.ShowMessage("Damage to char: " + iDamage.ToString(), 2);
							}//show message
						}//gothit
						else
							CEcalpon.ShowMessage(oMonster.Name + " missed!", 1);
					}//attack branch 
				}//check for hitpoints > 0
			}//foreach

			NextChar();
			CEcalpon.FightMonstMove = false;
			CEcalpon.FightCharMove = true;
			CEcalpon.FightCharMoveOrder = CEcalpon.gobjParty.FindNextConsciousChar(1);

		}

		/// <summary>
		/// Validate the character's attack.
		/// </summary>
		/// <param name="Row"></param>
		/// <param name="Col"></param>
		public void ValidateAttack(int Row, int Col) 
		{
			CCharacter oCharacter = CEcalpon.gobjParty.GetCharacterByOrder(CEcalpon.FightCharMoveOrder);
			CMonster oTargetMonster = null;

			int
				iDamage = 0;
			bool
				bAttacked = false;

			if(oCharacter.WeaponID == 99) 
			{
				CEcalpon.ShowMessage("No weapon!", 2);
				return;
			}

			if(oCharacter.Weapon.GroupHit) 
			{
				bAttacked = true;
				foreach(CMonster oMonster in oMonsters) 
				{
					if(oMonster.HitPoints > 0) 
					{
						oTargetMonster = oMonster;
						if(GotHit(oTargetMonster, oCharacter, Victim.MONSTER)) 
						{
							iDamage = ComputeDamage(oCharacter.InternalIndex, Victim.CHARACTER);
							oMonster.HitPoints -= iDamage;
							//grant experience if the monster is dead
							if(oMonster.HitPoints<=0) 
							{
								oCharacter.Experience += oMonster.ExperienceValue;
								FightOverlay[oMonster.FightRow, oMonster.FightCol] = 0;
								BodyCount++;
								CEcalpon.ShowMessage(oMonster.Name + " killed!", 2);
							}
							else
							{
								CEcalpon.ShowMessage("Damage: " + iDamage.ToString(), 2);
							}//check for monster dead
							CheckBattleStatus();
						}
						else
							CEcalpon.ShowMessage("Missed.", 1);
					}//check for if the monster is still in combat
				}//group hit processing
			}//check for group hit

			if(oCharacter.Weapon.Ranged && !bAttacked) 
			{
				bAttacked = true;

				int 
					rRow = 0,
					rCol = 0,
					sRow = oCharacter.FightRow,
					sCol = oCharacter.FightCol;
				bool bTarget = false;

				//find target monster
				if(Row==-1) //north
				{
					for(int dRow=sRow; dRow>-1; dRow--)
						if((FightOverlay[dRow, sCol] < 60) && (FightOverlay[dRow, sCol] > 0))
						{
							rRow = dRow;
							rCol = sCol;
							break;
						}
					bTarget = true;
				}
				if(Row==1 && !bTarget) //south
				{
					for(int dRow=sRow; dRow<19; dRow++)
						if((FightOverlay[dRow, sCol] < 60) && (FightOverlay[dRow, sCol] > 0))
						{
							rRow = dRow;
							rCol = sCol;
							break;
						}
					bTarget = true;
				}
				if(Col==-1 && !bTarget) //west
				{
					for(int dCol=sCol; dCol<19; dCol++)
						if((FightOverlay[sRow, dCol] < 60) && (FightOverlay[sRow, dCol] > 0))
						{
							rRow = sRow;
							rCol = dCol;
							break;
						}
					bTarget = true;
				}
				if(Col==1 && !bTarget) //east
				{
					for(int dCol=sCol; dCol>-1; dCol--)
						if((FightOverlay[sRow, dCol] < 60) && (FightOverlay[sRow, dCol] > 0))
						{
							rRow = sRow;
							rCol = dCol;
							break;
						}
					bTarget = true;
				}
				if((rRow > 0) && (rCol > 0)) 
				{
					foreach(CMonster oMonster in oMonsters)
					{
						if((oMonster.FightCol == rCol) && (oMonster.FightRow == rRow)) 
						{
							if(oMonster.HitPoints > 0) 
							{
								oTargetMonster = oMonster;
								if(GotHit(oTargetMonster, oCharacter, Victim.MONSTER)) 
								{
									iDamage = ComputeDamage(oCharacter.InternalIndex, Victim.CHARACTER);
									oMonster.HitPoints -= iDamage;
									//grant experience if the monster is dead
									if(oMonster.HitPoints<=0) 
									{
										oCharacter.Experience += oMonster.ExperienceValue;
										FightOverlay[oMonster.FightRow, oMonster.FightCol] = 0;
										BodyCount++;
										CEcalpon.ShowMessage(oMonster.Name + " killed!", 2);
									}
									else
									{
										CEcalpon.ShowMessage("Damage: " + iDamage.ToString(), 2);
									}//check for monster dead
									CheckBattleStatus();
								}
								else
									CEcalpon.ShowMessage("Missed.", 1);
							}//check for if the monster is still in combat
						}//check for equal coords
					}//find the right monster
				}//found a monster on a cardinal vector (N,S,E,W)
			}//ranged weapon attack

			//hand-to-hand attack
			if(!bAttacked) 
			{
				foreach(CMonster oMonster in oMonsters) 
				{
					if((oMonster.FightCol == oCharacter.FightCol + Col) &&
						(oMonster.FightRow == oCharacter.FightRow + Row)) 
					{
						if(oMonster.HitPoints > 0) 
						{
							oTargetMonster = oMonster;
							if(GotHit(oTargetMonster, oCharacter, Victim.MONSTER)) 
							{
								iDamage = ComputeDamage(oCharacter.InternalIndex, Victim.CHARACTER);
								oMonster.HitPoints -= iDamage;
								//grant experience if the monster is dead
								if(oMonster.HitPoints<=0) 
								{
									oCharacter.Experience += oMonster.ExperienceValue;
									FightOverlay[oMonster.FightRow, oMonster.FightCol] = 0;
									BodyCount++;
									CEcalpon.ShowMessage(oMonster.Name + " killed!", 2);
								}
								else
								{
									CEcalpon.ShowMessage("Damage: " + iDamage.ToString(), 2);
								}//check for monster dead
								CheckBattleStatus();
							}
							else
								CEcalpon.ShowMessage("Missed.", 1);
						}//check for if the monster is still in combat
					}//check for correct monster
				}//find correct monster to hit
			}//hand-to-hand attack

			CEcalpon.FightCharAttack = false;
			NextChar();

		}

		/// <summary>
		/// Validate the character's movement
		/// </summary>
		/// <param name="Row"></param>
		/// <param name="Col"></param>
		public void ValidateFightMove(int Row, int Col) 
		{
			CCharacter oCharacter = CEcalpon.gobjParty.GetCharacterByOrder(CEcalpon.FightCharMoveOrder);

			if(((Row + oCharacter.FightRow) >= FightOverlay.GetUpperBound(0)) ||
				((Row + oCharacter.FightRow) <= FightOverlay.GetLowerBound(0))) 
			{
				CEcalpon.gMoveMessage = "Can't escape!!";
				return;
			}

			if(((Col + oCharacter.FightCol) >= FightOverlay.GetUpperBound(1)) ||
				((Col + oCharacter.FightRow) <= FightOverlay.GetLowerBound(1))) 
			{
				CEcalpon.gMoveMessage = "Can't escape!!";
				return;
			}

			if(FightOverlay[Row + oCharacter.FightRow, Col + oCharacter.FightCol] != 0) 
			{
				CEcalpon.gMoveMessage = "Blocked";
				return;
			}

			//Valid move
			FightOverlay[oCharacter.FightRow, oCharacter.FightCol] = 0;
			oCharacter.FightRow += Row;
			oCharacter.FightCol += Col;
			FightOverlay[oCharacter.FightRow, oCharacter.FightCol] = 80 + oCharacter.ClassID;

			//Next character
			NextChar();
		}

		/// <summary>
		/// Find and hit the character in the overlay
		/// </summary>
		/// <param name="Monster"></param>
		/// <param name="VictimRow"></param>
		/// <param name="VictimCol"></param>
		/// <returns></returns>
		public bool TargetAcquisition(CMonster Monster, out int VictimRow, out int VictimCol) 
		{
			bool 
				bTarget = false,
                mNorth = false,
				mSouth = false,
				mEast = false,
				mWest = false;
			int
				r = Monster.FightRow,
				c = Monster.FightCol;

			//Default the out parameters...a limitation of c#
			VictimRow = VictimCol = 0;

			if((r-1)>0) //Check the north
			{
				if(FightOverlay[r-1,c]>=80) 
				{
					if(CEcalpon.gobjParty.FightFindCharacter(r-1,c).HitPoints>0)
						mNorth = true;
						
					if(CEcalpon.gobjParty.FightFindCharacter(r-1,c).HitPoints ==
						CEcalpon.gobjParty.FightFindCharacter(r-1,c).MaxHitPoints) 
					{
						VictimRow = r-1;
						VictimCol = c;
						return true;
					}
				}
			}
			if((r+1)>0) //Check the south
			{
				if(FightOverlay[r-1,c]>=80) 
				{
					if(CEcalpon.gobjParty.FightFindCharacter(r+1,c).HitPoints>0)
						mSouth = true;
						
					if(CEcalpon.gobjParty.FightFindCharacter(r+1,c).HitPoints ==
						CEcalpon.gobjParty.FightFindCharacter(r+1,c).MaxHitPoints) 
					{
						VictimRow = r+1;
						VictimCol = c;
						return true;
					}
				}
			}
			if((c-1)>0) //Check the west
			{
				if(FightOverlay[r,c-1]>=80) 
				{
					if(CEcalpon.gobjParty.FightFindCharacter(r,c-1).HitPoints>0)
						mWest = true;
						
					if(CEcalpon.gobjParty.FightFindCharacter(r,c-1).HitPoints ==
						CEcalpon.gobjParty.FightFindCharacter(r,c-1).MaxHitPoints) 
					{
						VictimRow = r;
						VictimCol = c-1;
						return true;
					}
				}
			}
			if((c+1)>0) //Check the east
			{
				if(FightOverlay[r,c+1]>=80) 
				{
					if(CEcalpon.gobjParty.FightFindCharacter(r,c+1).HitPoints>0)
						mEast = true;
						
					if(CEcalpon.gobjParty.FightFindCharacter(r,c+1).HitPoints ==
						CEcalpon.gobjParty.FightFindCharacter(r,c+1).MaxHitPoints) 
					{
						VictimRow = r;
						VictimCol = c+1;
						return true;
					}
				}
			}

			if(mSouth) 
			{
				VictimRow = r+1;
				VictimCol = c;
				bTarget = true;
			}
			if(mWest && !bTarget) 
			{
				VictimRow = r;
				VictimCol = c-1;
				bTarget = true;
			}
			if(mEast && !bTarget) 
			{
				VictimRow = r;
				VictimCol = c+1;
				bTarget = true;
			}
			if(mNorth && !bTarget) 
			{
				VictimRow = r-1;
				VictimCol = c;
			}
			

			return (mNorth || mSouth || mEast || mWest);

		}


		/// <summary>
		/// Determines if the combat is over.
		/// </summary>
		public void CheckBattleStatus() 
		{
			if(BodyCount==oMonsters.Count) 
			{
				CEcalpon.DistributeLoot(oLoot);

				if(eFightLocale==Locale.OUTSIDE) 
				{
					CEcalpon.RemoveSurfaceMonster();
				}
				if(eFightLocale==Locale.ENCOUNTER) 
				{
					CEcalpon.oEncounter.RemoveEncounterMonster();
				}
				CEcalpon.FightMode = false;
				CEcalpon.TimeSlip = false;
				CEcalpon.gMessage = "";
				CEcalpon.gHitMessage = "";
				CEcalpon.gAttackMessage = "";
				CEcalpon.gMoveMessage = "";
				CEcalpon.gobjParty.CheckForLevelIncreases();
			}
		}


		/// <summary>
		/// Move the monster and update the overlay.
		/// </summary>
		/// <param name="Monster"></param>
		/// <param name="rTarget"></param>
		/// <param name="cTarget"></param>
		public void ValidateMonsterMove(CMonster Monster, int rTarget, int cTarget) 
		{
			int
				r = 0,
				c = 0,
				rmod = 0,
				cmod = 0;
			bool bMoved = false;

			r = Monster.FightRow;
			c = Monster.FightCol;

			if(r < rTarget) //character is below us
				rmod = 1;
			if(r == rTarget) //we're on the same row
				rmod = 0;
			if(r > rTarget) //he's above us
				rmod = -1;

			if(c < cTarget) //character is to the right of us
				cmod = 1;
			if(c == cTarget) // we're on the same col
				cmod = 0;
			if(c > cTarget) //char is to the left of us
				cmod = -1;

			if(FightOverlay[r + rmod, c + cmod] == 0) //diagonal move
			{
				FightOverlay[Monster.FightRow, Monster.FightCol] = 0;
				FightOverlay[r + rmod, c + cmod] = Monster.ID;
				Monster.FightRow = r + rmod;
				Monster.FightCol = c + cmod;
				bMoved = true;
			}

			if(!bMoved && FightOverlay[r + rmod, c] == 0) //row move
			{
				FightOverlay[Monster.FightRow, Monster.FightCol] = 0;
				FightOverlay[r + rmod, c] = Monster.ID;
				Monster.FightRow = r + rmod;
				Monster.FightCol = c;
				bMoved = true;
			}

			if(!bMoved && FightOverlay[r, c + cmod] == 0) //col move 
			{
				FightOverlay[Monster.FightRow, Monster.FightCol] = 0;
				FightOverlay[r, c + cmod] = Monster.ID;
				Monster.FightRow = r;
				Monster.FightCol = c + cmod;
				bMoved = true;
			}

		}

		/// <summary>
		/// Determine if a defender got hit.
		/// </summary>
		/// <param name="Monster"></param>
		/// <param name="Character"></param>
		/// <param name="Defender"></param>
		/// <returns></returns>
		public bool GotHit(CMonster Monster, CCharacter Character, Victim Defender) 
		{
			bool bGotHit = false;
			int 
				iLevel = 0, 
				iToHit = 0, 
				iModifier = 0,
				iRoll = 0;
			string sSQL = "";
			OleDbDataReader drReader =  null;
			CDataAccess oDataAccess = new CDataAccess();

			//Get the "ToHit" figure to beat wit a random roll
			switch(Defender) 
			{
				case Victim.CHARACTER:
					iLevel = Monster.Level;
					sSQL = "SELECT * FROM AttackMatrixFighter WHERE " +
						" AC = " + Character.ArmorClass.ToString();
					break;
				case Victim.MONSTER:
					iLevel = Character.Level;
					switch(Character.ClassID) 
					{
						case (int)CCharacter.eClass.Fighter:
							sSQL = "SELECT * FROM AttackMatrixFighter WHERE " +
								" AC = " + Monster.ArmorClass.ToString();
							break;
						case (int)CCharacter.eClass.Wizard:
							sSQL = "SELECT * FROM AttackMatrixMage WHERE " +
								" AC = " + Monster.ArmorClass.ToString();
							break;
						case (int)CCharacter.eClass.Cleric:
							sSQL = "SELECT * FROM AttackMatrixCleric WHERE " +
								" AC = " + Monster.ArmorClass.ToString();
							break;
					}// switch
					
					break;
			}
			//Execute the SQL statement
			oDataAccess.FillDataReader(out drReader, sSQL);

			//Look up the to hit rating for the aggressor
			if(drReader.Read()) 
			{
				if(iLevel<20) 
					iToHit = (int)drReader[iLevel];
				else
					iToHit = (int)drReader["PLUS"];
			}
			
			//Close our connection
			drReader.Close();
			oDataAccess = null;

			//Does the weapon have a strength or magic bonus
			if(Defender==Victim.MONSTER)
				iModifier = Character.Weapon.Modifier;
			else
				iModifier = Monster.Weapon.Modifier;
			
			//Subtract any hit modifiers
			iToHit -= iModifier;

			//Roll the 20-sided die
			iRoll = CEcalpon.MyRand(20);

			if(iRoll >= iToHit) 
			{
				bGotHit = true;
				CEcalpon.PlaySound(CEcalpon.Sounds.HIT);
			}

			return bGotHit;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="MonsterID"></param>
		/// <param name="MinMonster"></param>
		/// <param name="MaxMonster"></param>
		/// <param name="Location"></param>
		/// <param name="mRow"></param>
		/// <param name="mCol"></param>
		/// <param name="bgrnd"></param>
		/// <param name="rndbg"></param>
		public CCombat(int MonsterID, int MinMonster, int MaxMonster, Locale Location,
			int mRow, int mCol, int bgrnd, int rndbg)
		{
			//DELETE LINES AFTER DEBUGGING
			MinMonster = 1; MaxMonster = 1;


			int iTreasureType = 0;

			//Cancel any spells initiated before the fight
			CEcalpon.SpellPending = false;
			CEcalpon.SpellParm = "";
			
			//if the backgrounds were defaulted to -1, use trees and grass
			//as fight tiles
			if(bgrnd==-1) 
			{
				bgrnd = 0;
			}
			if(rndbg==-1)
			{
				rndbg = 4;
			}


			//What kind of fight are we in
			eFightLocale = (Locale)Location;

			
			//Save the coords of the monster and treasure type for removal upon winning the fight
			//Outside Encounter
			if(eFightLocale == Locale.OUTSIDE) 
			{
				foreach(CMonster oMonster in CEcalpon.gobjSurfaceMonsters) 
				{
					if(oMonster.FightCol == mCol && oMonster.FightRow == mRow) 
					{
						FightRow = mRow;
						FightCol = mCol;
						break;
					}
				}
			}

			//Inside an Encounter
			if(eFightLocale == Locale.ENCOUNTER) 
			{
				foreach(CMonster oMonster in CEcalpon.oEncounter.Monsters)
				{
					if(oMonster.FightCol == mCol && oMonster.FightRow == mRow) 
					{
						FightRow = mRow;
						FightCol = mCol;
						break;
					}
				}
			}

			//Setup fight matrices
			for(int Row=0; Row<19; Row++)
				for(int Col=0; Col<19; Col++) 
				{
					if(CEcalpon.MyRand(10) == 10) 
						FightBase[Row, Col] = rndbg;
					else
						FightBase[Row, Col] = bgrnd;
					FightOverlay[Row, Col] = 0;
				}
		
			//What is a fight without monsters!!
			oMonsters = new CMonsters();
			int iMonsterCount = CEcalpon.MyRand(MinMonster, MaxMonster);
			oMonsters.LoadMonstersForCombat(iMonsterCount, MonsterID);

			//Place Monsters in Overlay
			if(iMonsterCount > 0) 
				FightOverlay[4,8] = MonsterID;
			if(iMonsterCount > 1) 
				FightOverlay[4,10] = MonsterID;
			if(iMonsterCount > 2) 
				FightOverlay[3,7] = MonsterID;
			if(iMonsterCount > 3) 
				FightOverlay[3,11] = MonsterID;
			if(iMonsterCount > 4) 
				FightOverlay[3,9] = MonsterID;
			if(iMonsterCount > 5) 
				FightOverlay[2,9] = MonsterID;
			if(iMonsterCount > 6) 
				FightOverlay[2,6] = MonsterID;
			if(iMonsterCount > 7) 
				FightOverlay[2,12] = MonsterID;

			//Place characters on fight grid
			for(int Order=1; Order<6; Order++) 
			{
				foreach(CCharacter oCharacter in CEcalpon.gobjParty.Characters) 
				{
					if(oCharacter.Order==Order) 
					{
						switch(Order) 
						{
							case 1:
								FightOverlay[13,8] = 80 + oCharacter.ClassID;
								oCharacter.FightRow = 13;
								oCharacter.FightCol = 8;
								break;
							case 2:
								FightOverlay[13,10] = 80 + oCharacter.ClassID;
								oCharacter.FightRow = 13;
								oCharacter.FightCol = 10;
								break;
							case 3:
								FightOverlay[15,7] = 80 + oCharacter.ClassID;
								oCharacter.FightRow = 15;
								oCharacter.FightCol = 7;
								break;
							case 4:
								FightOverlay[15,9] = 80 + oCharacter.ClassID;
								oCharacter.FightRow = 15;
								oCharacter.FightCol = 9;
								break;
							case 5:
								FightOverlay[15,11] = 80 + oCharacter.ClassID;
								oCharacter.FightRow = 15;
								oCharacter.FightCol = 11;
								break;
						} //switch
						break;
					} //order comparison
				} //character iteration
			} //order iteration

			CEcalpon.FightMode = true;
			CEcalpon.FightCharMove = true;

			CEcalpon.FightCharMoveOrder = CEcalpon.gobjParty.FindNextConsciousChar(1);
			CEcalpon.ShowMessage("Conflict!!!", 2);
			
			//Load up the loot!
			CMonster oTreasureTypeMonster = new CMonster();
			oTreasureTypeMonster.LoadMonsterByID(MonsterID);
			iTreasureType = oTreasureTypeMonster.TreasureType;
			oTreasureTypeMonster = null;
			oLoot = new CLoot(iTreasureType);

		}
	}
}

