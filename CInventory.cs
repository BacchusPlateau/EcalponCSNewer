using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;
using Ecalpon;

	public class CInventory : CollectionBase
	{
	
		public void Add(CItem oItem)
		{
			oItem.InternalIndex = List.Count;
			List.Add(oItem);
		}
		
		public void Remove(int index)
		{
			// Check to see if there is a widget at the supplied index.
			if (index > Count - 1 || index < 0)
				// If no widget exists, a messagebox is shown and the operation 
				// is cancelled.
			{
				System.Windows.Forms.MessageBox.Show("Index not valid!");
				return;
			}
			List.RemoveAt(index);

			//Reindex
			CItem oItem=null;
			for(int i=0; i<List.Count; i++) 
			{
				oItem = (CItem) List[i];
				oItem.InternalIndex = i;
			}

		}

		public CItem Item(int Index)
		{
			// The appropriate item is retrieved from the List object and
			// explicitly cast to the CItem type, then returned to the 
			// caller.
			return (CItem) List[Index];
		}

		public void LoadInventoryByCharacterID(int ID) 
		{
			string sSQL;
			OleDbDataReader drReader;
			CDataAccess oDataAccess = new CDataAccess();
			CItem oItem;

			// Item type 0 = Weapons
			// Item type 1 = Armor
			// Item type 2 = Misc. / other items
			// Type_ID is a foreign key to either the Weapons, armor or Misc/ other items

			sSQL = "select inv.item_id, inv.inuse, inv.charges, i.name, " +
				 "i.[Type], i.Type_ID, i.Class  " +
				"from inventory inv, item i " +
				"where(i.id = inv.item_id) " +
				"and inv.chardata_id = " + ID.ToString();

			try 
			{
				oDataAccess.FillDataReader(out drReader, sSQL);

				while(drReader.Read()) 
				{
					oItem = new CItem();
					oItem.ID = (System.Int16) drReader["Item_ID"];
					oItem.InUse = (bool) drReader["InUse"];
					oItem.Charges = (System.Int16) drReader["Charges"];
					oItem.Name = drReader["Name"].ToString();
					oItem.InternalIndex = List.Count;
					oItem.ItemCharacterClass = (System.Int16) drReader["Class"];
					oItem.ItemType = (System.Int16) drReader["Type"];
					oItem.ItemTypeID = (System.Int16) drReader["Type_ID"];
					
					List.Add(oItem);
				}

				drReader.Close();
				oDataAccess = null;
			}
			
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}

		}

	}

