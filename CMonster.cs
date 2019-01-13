using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public class CMonster : CCharacter
{
	private int iExpValue;
	private int iMagicClass;
	private int iDamageModifier;
	private int iTreasureType;

	public int ExperienceValue 
	{
		get 
		{
			return iExpValue;
		}
		set 
		{
			iExpValue = value;
		}
	}

	public int TreasureType 
	{
		get 
		{
			return iTreasureType;
		}
		set 
		{
			iTreasureType = value;
		}
	}

	public int DamageModifier 
	{
		get 
		{
			return iDamageModifier;
		}
		set 
		{
			iDamageModifier = value;
		}
	}

	public int MagicClass 
	{
		get 
		{
			return iMagicClass;
		}
		set 
		{
			iMagicClass = value;
		}
	}

	public void LoadMonsterByID(int ID) 
	{
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
	
		sSQL = "SELECT * FROM Monster WHERE ID = " + ID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			drReader.Read();
			
			this.Name = drReader["Name"].ToString();
			this.ID = (System.Int16) drReader["ID"];
			this.HitPoints = (System.Int16) drReader["HP"];
			this.MaxHitPoints = this.HitPoints;
			this.Level = (System.Int16) drReader["Level"];
			iExpValue = (System.Int16) drReader["ExpValue"];
			this.Strength = 15;
			this.Charisma = 15;
			this.Charisma = 15;
			this.Intelligence = 15;
			this.Dexterity = 15;
			this.Wisdom = 15;
			this.WeaponID = (System.Int16) drReader["Weapon_ID"];
			this.ArmorClass = (System.Int16) drReader["AC"];
			this.FightRow = 0;
			this.FightCol = 0;
			iDamageModifier = (System.Int16) drReader["DamageModifier"];
			iTreasureType = (int) drReader["TreasureType"];

			drReader.Close();
			oDataAccess = null;
			
			this.ComputeAC();
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}


	
	}

}
