using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

public class CSpell
{
	private int iID;
	private string sName;
	private int iCount;
	private string sSpellType;
	private int iModifier;
	private int iSpellClass;
	private int iLevel;
	private string sAttribute;
	private bool bGroupSpell;
	private int iIndex;

	/// <summary>
	/// Accessors
	/// </summary>
	public string NameDisplay 
	{
		get
		{
			return iLevel.ToString() + "-" + sName +
				"(" + iCount.ToString() + ")";
		}
	}

	public string NameAvailable 
	{
		get
		{
			return iLevel.ToString() + "-" + sName ;
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

	public bool GroupSpell 
	{
		get
		{
			return bGroupSpell;
		}
		set 
		{
			bGroupSpell = value;
		}
	}

	public string Attribute 
	{
		get 
		{
			return sAttribute;
		}
		set 
		{
			sAttribute = value;
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

	public int SpellClass 
	{
		get 
		{
			return iSpellClass;
		}
		set 
		{
			iSpellClass = value;
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

	public string SpellType 
	{
		get 
		{
			return sSpellType;
		}
		set 
		{
			sSpellType = value;
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

	public int Count 
	{
		get 
		{
			return iCount;
		}
		set
		{
			iCount = value;
		}
	}

	/// <summary>
	/// Load the object by a spell id
	/// </summary>
	/// <param name="id"></param>
	public void LoadByID(int id) 
	{
		string sSQL;
		OleDbDataReader drReader;
		CDataAccess oDataAccess = new CDataAccess();
	
		sSQL = "SELECT * FROM SPELLS WHERE ID = " + id.ToString();

		try 
		{
			oDataAccess.FillDataReader(out drReader, sSQL);

			drReader.Read();

			this.ID = (int) drReader["ID"];
			this.Count = 0;  //when displaying available spells, count doesn't apply
			this.Name = drReader["Name"].ToString();
			this.SpellType = drReader["Type"].ToString();
			this.Modifier = (int) drReader["Modifier"];
			this.SpellClass = (int) drReader["Class"];
			this.Level = (int) drReader["Level"];
			this.GroupSpell = ((int) drReader["Group"]) == 0 ? false : true;
			this.Attribute = drReader["Attribute"].ToString();

			drReader.Close();
			oDataAccess = null;
		}
			
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}

	}

}