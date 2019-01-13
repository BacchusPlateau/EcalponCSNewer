using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Ecalpon;

public class CItem
{
	private int iCharges;
	private int iItemID;
	private string sItem;
	private bool bInUse;
	private int iIndex;
	private int iItemType;
	private int iItemTypeID;
	private int iItemCharClass;
	private int iRating;
	private int iModifier;
	private string sCategory;
	private int iDamageMin;
	private int iDamageMax;
	private bool bRanged;
	private bool bGroupHit;
	private int iExperienceValue;
	private int iPrice;
	private string sNamePrice;

		public string NamePrice
		{
			get
			{
				return (sItem + " / " + iPrice.ToString());
			}
		}

		public int Price
		{
			get
			{
				return iPrice;
			}
			set
			{
				iPrice = value;
			}
		}				

		public int ExperienceValue
		{
			get
			{
				return iExperienceValue;
			}
			set
			{
				iExperienceValue = value;
			}
		}

		public bool InUse 
		{
			get
			{
				return bInUse;
			}
			set 
			{
				bInUse = value;
			}
		}

		public int Charges
		{
			get 
			{
				return iCharges;
			}
			set
			{
				iCharges = value;
			}
		}

		public int ID 
		{
			get 
			{
				return iItemID;
			}
			set
			{
				iItemID = value;
			}
		}

		public bool Ranged
		{
			get
			{
				return bRanged;
			}
			set
			{
				bRanged = value;
			}
		}

		public bool GroupHit
		{
			get
			{
				return bGroupHit;
			}
			set
			{
				bGroupHit = value;
			}
		}

		public string Category 
		{
			get
			{
				return sCategory;
			}
			set
			{
				sCategory = value;
			}
		}

		public int DamageMin 
		{
			get
			{
				return iDamageMin;
			}
			set
			{
				iDamageMin = value;
			}
		}

		public int DamageMax 
		{
			get 
			{
				return iDamageMax;
			}
			set
			{
				iDamageMax = value;
			}
		}

		public int Rating
		{
			get
			{
				return iRating;
			}
			set
			{
				iRating = value;
			}
		}

		public int Modifier
		{
			get
			{
				return iModifier;
			}
			set
			{
				iModifier = value;
			}
		}

		public int ItemType 
		{
			get
			{
				return iItemType;
			}
			set
			{
				iItemType = value;
			}
		}

		public int ItemCharacterClass 
		{
			get
			{
				return iItemCharClass;
			}
			set
			{
				iItemCharClass = value;
			}
		}

		public int ItemTypeID 
		{
			get
			{
				return iItemTypeID;
			}
			set
			{
				iItemTypeID = value;
				GetSetItemSpecificProps();
			}
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

		public string Name 
		{
			get 
			{
				string sTempName = sItem;
				if((iModifier != 0) && (iCharges == -1)) 
				{
					sTempName = "+" + iModifier.ToString() + " " + sItem;
				}
				if(bInUse) 
				{
					return ("* " + sTempName);
				} 
				else 
				{
					return sTempName;
				}

			}
			set
			{
				sItem = value;
			}
		}
		
		public void LoadItemByID(int ID) 
		{
			string sSQL;
			OleDbDataReader drReader = null;
			CDataAccess oDataAccess = new CDataAccess();
			int
				iMinCharges = 0,
				iMaxCharges = 0;

			sSQL = "SELECT Class, Type, Type_ID, MinCharges, MaxCharges, Name " +
				"FROM Item WHERE ID = " + ID.ToString();

			try
			{
				oDataAccess.FillDataReader(out drReader, sSQL);
				drReader.Read();
				
				this.ID = (System.Int16)ID;
				this.InUse = false;
				iMinCharges = (System.Int16) drReader["MinCharges"];
				iMaxCharges = (System.Int16) drReader["MaxCharges"];
				this.Charges = Ecalpon.CEcalpon.MyRand(iMinCharges, iMaxCharges);
				this.Name = drReader["Name"].ToString();
				this.InternalIndex = 0;
				this.ItemCharacterClass = (System.Int16) drReader["Class"];
				this.ItemType = (System.Int16) drReader["Type"];
				this.ItemTypeID = (System.Int16) drReader["Type_ID"];
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			oDataAccess = null;
		}

		private void GetSetItemSpecificProps() 
		{

			string sSQL;
			OleDbDataReader drReader = null;
			CDataAccess oDataAccess = new CDataAccess();

			try 
			{
				switch (iItemType) 
				{
					case 0 : //Weapon
					{
						sSQL = "SELECT * FROM WEAPON WHERE ID = " +
							iItemTypeID.ToString();
						oDataAccess.FillDataReader(out drReader, sSQL);
						drReader.Read();
						iModifier = (System.Int16) drReader["Modifier"];
						iDamageMin = (System.Int16) drReader["DamMin"];
						iDamageMax = (System.Int16) drReader["DamMax"];
						bGroupHit = (bool) drReader["GroupHit"];
						bRanged = (bool) drReader["Ranged"];
						iPrice = (int) drReader["Price"];
						iExperienceValue = (int) drReader["Exp"];
						break;
					}
					case 1 : //Armor
					{
						sSQL = "SELECT * FROM ARMOR WHERE ID = " + iItemTypeID.ToString();
						oDataAccess.FillDataReader(out drReader, sSQL);
						drReader.Read();
						iRating = (System.Int16) drReader["Rating"];
						iModifier = (System.Int16) drReader["Modifier"];
						iPrice = (int) drReader["Price"];
						iExperienceValue = (int) drReader["Exp"];
						break;
					}
					case 2 : //Other
					{
						sSQL = "SELECT * FROM OTHERITEMS WHERE ID = " + iItemTypeID.ToString();
						oDataAccess.FillDataReader(out drReader, sSQL);
						drReader.Read();
						iRating = (System.Int16) drReader["Rating"];
						sCategory = drReader["Category"].ToString();
						iPrice = (int) drReader["Price"];
						iExperienceValue = (int) drReader["Exp"];
						break;
					}
				}
				
				drReader.Close();
				oDataAccess = null;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		public CItem()
		{
			
		}
	}


