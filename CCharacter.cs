using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public class CCharacter
{

	private int iID;
	private string sName;
	private int iHP;
	private int iMaxHP;
	private int iLevel;
	private int iGold;
	private int iExperience;
	private int iStrength;
	private int iCharisma;
	private int iConstitution;
	private int iDexterity;
	private int iWisdom;
	private int iWeapon;
	private string sWeapon;
	private int iIntelligence;
	private int iArmorClass;
	private int iFightRow;
	private int iFightCol;
	private int iOrder;
	private int iIndex;
	private CInventory oInv;
	private CSpells oSpells;

	public enum eClass { Fighter, Wizard, Cleric };
	private eClass iClass;

	public CCharacter()
	{
		oInv = new CInventory();	
		oSpells = new CSpells();	
	}

	public int InternalIndex 
	{
		get 
		{
			return iIndex;
		}
		set 
		{
			iIndex = value;
		}
	}

	public int Intelligence 
	{
		get 
		{
			return iIntelligence;
		}
		set 
		{
			iIntelligence = value;
		}
	}

	public CInventory Inventory 
	{
		get 
		{
			return oInv;
		}
	}

	public CSpells Spells 
	{
		get 
		{
			return oSpells;
		}
	}

	public int FightCol 
	{
		get 
		{
			return iFightCol;
		}
		set
		{
			iFightCol = value;
		}
	}

	public int FightRow
	{
		get 
		{
			return iFightRow;
		}
		set 
		{
			iFightRow = value;
		}
	}

	public int ArmorClass 
	{
		get 
		{
			return iArmorClass;
		}
		set 
		{
			iArmorClass = value;
		}
	}

	public int WeaponID 
	{
		get 
		{
			return iWeapon; 
		}
		set 
		{
			iWeapon = value; 
			sWeapon = LoadWeaponName(iWeapon);
		}
	}

	public string WeaponName 
	{
		get 
		{
			return sWeapon;
		}
		set 
		{
			sWeapon = value;
		}
	}

	public int Wisdom 
	{
		get 
		{
			return iWisdom;
		}
		set 
		{
			iWisdom = value;
		}
	}

	public int Dexterity 
	{
		get 
		{
			return iDexterity;
		} 
		set 
		{
			iDexterity = value;
		}
	}
	
	public int Constitution 
	{
		get 
		{
			return iConstitution;
		}
		set 
		{
			iConstitution = value;
		}
	}

	public int Charisma 
	{
		get 
		{
			return iCharisma;
		}
		set 
		{
			iCharisma = value;
		}
	}

	public int Strength 
	{
		get 
		{
			return iStrength;
		}
		set 
		{
			iStrength = value;
		}
	}

	public int Experience 
	{
		get 
		{
			return iExperience;
		}
		set 
		{
			iExperience = value;
		}
	}

	public int Gold 
	{
		get 
		{
			return iGold;
		}
		set 
		{
			iGold = value;
		}
	}

	public int Level 
	{
		get 
		{
			return iLevel;
		}
		set 
		{
			iLevel = value;
		}
	}

	public int Order 
	{
		get 
		{
			return iOrder;
		}
		set 
		{
			iOrder = value;
		}
	}

	public int MaxHitPoints 
	{
		get 
		{
			return iMaxHP;
		}
		set 
		{
			iMaxHP = value;
		}
	}

	public int HitPoints 
	{
		get 
		{
			return iHP;
		}
		set 
		{
			iHP = value;
		}
	}

	public string ClassName 
	{
		get 
		{
			string s="";
			switch (iClass)
			{
				case eClass.Fighter: 
					s = "Fighter";
					break;
				case eClass.Wizard:
					s =  "Wizard";
					break;
				case eClass.Cleric:
					s =  "Cleric";
					break;
			}
			return s;
		}
	}

	public int ClassID 
	{
		get 
		{
			return (int) iClass;
		}
	}

	public int ID 
	{
		get
		{
			return iID;
		}
		set 
		{
			iID = value;
		}
	}

	public string Name 
	{
		get 
		{
			return sName;
		}
		set 
		{
			sName = value;
		}
	}

	public CItem Weapon 
	{
		get 
		{
			CItem o=null;

			foreach (CItem oItem in oInv) 
				if(oItem.ID == this.WeaponID && oItem.InUse) 
				{
					o = oItem;
				}

			return o;
		}
	}

	private string LoadWeaponName(int ID) 
	{
		string sSQL, sWeapon="";
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();

		sSQL = "select * from weapon where id = " + ID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);
			drReader.Read();
			sWeapon = drReader["Name"].ToString();
			drReader.Close();
			oDataAccess = null;
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}

		return sWeapon;
	}

	public void ComputeAC() 
	{

		this.ArmorClass = 10;

		foreach(CItem oItem in oInv) 
		{
			if(oItem.ItemType==1 && oItem.InUse)
				this.ArmorClass -= oItem.Rating;
			if(oItem.ItemType==2 && oItem.Category=="AC" && oItem.InUse)
				this.ArmorClass -= oItem.Rating;
		}
	}

	public void LoadCharacterByID(int ID) 
	{
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
	
		sSQL = "SELECT * FROM Chardata WHERE ID = " + ID.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			drReader.Read();
			
			sName = drReader["Name"].ToString();
			iID = (System.Int16) drReader["ID"];
			iHP = (System.Int16) drReader["HP"];
			iMaxHP = (System.Int16) drReader["MaxHP"];
			iLevel = (System.Int16) drReader["Level"];
			iGold = (int) drReader["Gold"];
			iExperience = (int) drReader["Experience"];
			iStrength = (System.Int16) drReader["ST"];
			iCharisma = (System.Int16) drReader["CH"];
			iConstitution = (System.Int16) drReader["CO"];
			iIntelligence = (System.Int16) drReader["IN"];
			iDexterity = (System.Int16) drReader["DX"];
			iWisdom = (System.Int16) drReader["WI"];
			iWeapon = (System.Int16) drReader["Weapon_ID"];
			iArmorClass = (System.Int16) drReader["AC"];

			iFightRow = 0;
			iFightCol = 0;
			iClass = (eClass) (System.Int16) drReader["Class"];

			drReader.Close();
			oDataAccess = null;

			sWeapon = LoadWeaponName(iWeapon);
			oSpells.LoadSpellsByCharacterID(iID);
			oInv.LoadInventoryByCharacterID(iID);
			this.ComputeAC();
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}


	}
}

