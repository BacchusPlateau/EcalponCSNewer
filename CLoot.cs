using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;

namespace Ecalpon
{
	/// <summary>
	/// Holds treasure after an encounter or fight
	/// </summary>
	public class CLoot
	{
		private ArrayList oItems;
		private int iGold;

		public ArrayList Items
		{
			get 
			{
				return oItems;
			}
		}

		public int Gold 
		{
			get
			{
				return iGold;
			}
		}

		public void AddItem(int ItemID) 
		{
			CItem oItem = new CItem();
			oItem.LoadItemByID(ItemID);
			oItems.Add(oItem);
		}

		public CLoot(int TreasureType)
		{
			oItems = new ArrayList();

			//if TreasureType is a zero, then the loot will be added manually
			if(TreasureType!=0)
			{
				string sSQL;
				int iMaxGold = 0,
					iMinGold = 0;

				CItem oItem = null;

				OleDbDataReader drReader;
				CDataAccess oDataAccess = new CDataAccess();

				sSQL = "SELECT * FROM TreasureType WHERE ID = " + TreasureType.ToString();

				oDataAccess.FillDataReader(out drReader, sSQL);

				drReader.Read();
				
				try 
				{
					iMaxGold = (int)drReader["MaxGold"];
					iMinGold = (int)drReader["MinGold"];
					if(iMaxGold > 0) 
						iGold = CEcalpon.MyRand(iMinGold, iMaxGold);
					else
						iGold = 0;

					//I know this is a HORRIBLE one-to-many implementation...
					for(int i=3; i<10; i++) 
					{
						if((int)drReader[i]!=-1) 
						{
							oItem = new CItem();
							oItem.LoadItemByID((int)drReader[i]);
							oItems.Add(oItem);
						}
						else
							break;
					}
				}
				catch(Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			}
		}
	}
}
