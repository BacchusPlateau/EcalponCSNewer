using System;
using DxVBLib;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Drawing;

public class CDirectx
{
	// Needed to clear up the Hbitmap unmanaged resource
	[System.Runtime.InteropServices.DllImport("gdi32.dll")]
	static extern bool DeleteObject(IntPtr hObject);

	/// <summary>
	/// CreateCompatibleDC
	/// </summary>
	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

	/// <summary>
	/// DeleteDC
	/// </summary>
	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern bool DeleteDC(IntPtr hdc);

	/// <summary>
	/// SelectObject
	/// </summary>
	[DllImport("gdi32.dll", ExactSpelling=true)]
	public static extern IntPtr SelectObject( IntPtr hDC,
		IntPtr hObject);

	/// <summary>
	/// CreateCompatibleBitmap
	/// </summary>
	[DllImport("gdi32.dll",
		 ExactSpelling=true,
		 SetLastError=true)]
	public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject,
		int width,
		int height);

	/// <summary>
	/// BitBlt
	/// </summary>
	[DllImport("gdi32.dll", ExactSpelling=true, SetLastError=true)]
	public static extern bool BitBlt(IntPtr hObject,
		int nXDest,
		int nYDest,
		int nWidth,
		int nHeight,
		IntPtr hObjSource,
		int nXSrc,
		int nYSrc,
		int dwRop);
	

	public DirectDraw7 dixuDDraw;
	public DirectDrawSurface7 dixuPrimarySurface;
	public DirectDrawSurface7 dixuBackBuffer;
	public DirectDrawClipper dixuClipper;

	public DDSURFACEDESC2 ddsdPrimarySurface;
	public DDSURFACEDESC2 ddsdBackBuffer;
	public RECT ScreenRect;
	public DirectX7 DX;
	public DirectSound DS;
	private DSBUFFERDESC oSoundDescription;

	public bool blnBackBufferClear;

	public void dixuInit(int Flags, Form frm, int Width, int Height,
		int BitsPerPixel, PictureBox pic) 
	{

		try 
		{
			DX = new DirectX7();
			dixuDDraw = DX.DirectDrawCreate("");
			frm.Show();

			dixuDDraw.SetCooperativeLevel(frm.Handle.ToInt32(), CONST_DDSCLFLAGS.DDSCL_NORMAL);

			ddsdPrimarySurface.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS;
			ddsdPrimarySurface.ddsCaps.lCaps =
				CONST_DDSURFACECAPSFLAGS.DDSCAPS_PRIMARYSURFACE;
			ddsdPrimarySurface.lWidth = pic.Width;
			ddsdPrimarySurface.lHeight = pic.Height;
			dixuPrimarySurface = dixuDDraw.CreateSurface(ref ddsdPrimarySurface);

			dixuClipper = dixuDDraw.CreateClipper(0);
			dixuClipper.SetHWnd(pic.Handle.ToInt32());
			dixuPrimarySurface.SetClipper(dixuClipper);

			ddsdBackBuffer.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | 
				CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT | CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
			ddsdBackBuffer.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN |
				CONST_DDSURFACECAPSFLAGS.DDSCAPS_SYSTEMMEMORY;
			ddsdBackBuffer.lWidth = pic.Width;
			ddsdBackBuffer.lHeight = pic.Height;

			dixuBackBuffer = dixuDDraw.CreateSurface(ref ddsdBackBuffer);

			//Initalize DirectSound
			InitializeSound(frm.Handle.ToInt32());
		}
		catch(Exception e)
		{
			MessageBox.Show(e.ToString());
		}

	}

	/// <summary>
	/// Initialize DirectSound
	/// </summary>
	/// <param name="Handle"></param>
	public void InitializeSound(int Handle) 
	{
		try 
		{
			DS = DX.DirectSoundCreate("");
			DS.SetCooperativeLevel(Handle,DxVBLib.CONST_DSSCLFLAGS.DSSCL_NORMAL);
		}
		catch(Exception e)
		{
			System.Diagnostics.Debug.WriteLine(e.ToString());
		}
			
	}

	/// <summary>
	/// Load a wav file into a sound buffer
	/// </summary>
	/// <param name="FileName"></param>
	/// <returns></returns>
	public DirectSoundBuffer LoadSoundFromWaveFile(string FileName)
	{

		DirectSoundBuffer oSound=null;
		WAVEFORMATEX oWaveFormat;
			
		string sPathToFile = Application.StartupPath + "\\resources\\" + FileName + ".wav";

		try
		{
			oSound = DS.CreateSoundBufferFromFile(sPathToFile, ref oSoundDescription,
				out oWaveFormat);
		}
		catch(Exception e)
		{
			System.Diagnostics.Debug.WriteLine(e.ToString());
		}

		return oSound;
	}

	/// <summary>
	/// Cleanup
	/// </summary>
	public void dixuDone() 
	{

		dixuClipper = null;
		dixuBackBuffer = null;
		dixuPrimarySurface = null;
		dixuDDraw = null;
		DS = null;
		DX = null;
	}

	/// <summary>
	/// Clear the back buffer
	/// </summary>
	public void dixuBackBufferClear() 
	{

		RECT rectTo;
			
		rectTo.Left = 0;
		rectTo.Top = 0;
		rectTo.Bottom = ddsdBackBuffer.lHeight;
		rectTo.Right = ddsdBackBuffer.lWidth;

		dixuBackBuffer.BltColorFill(ref rectTo, 0);

		blnBackBufferClear = false;
	}

	/// <summary>
	/// "Flip" the backbuffer onto the primary surface
	/// </summary>
	/// <param name="pic"></param>
	public void dixuBackBufferDraw(PictureBox pic) 
	{

		if (blnBackBufferClear) 
			dixuBackBufferClear();

		RECT dixuClientRect = new RECT();  //Initialization due to CS0165 error
		RECT dixuBackRect;
		int iReturnValue;

		dixuBackRect.Top = 0;
		dixuBackRect.Left = 0;
		dixuBackRect.Bottom = ddsdBackBuffer.lHeight;
		dixuBackRect.Right = ddsdBackBuffer.lWidth;

		try 
		{
			DX.GetWindowRect(pic.Handle.ToInt32(), ref dixuClientRect);

			iReturnValue = dixuPrimarySurface.Blt(ref dixuClientRect, dixuBackBuffer,
				ref dixuBackRect, CONST_DDBLTFLAGS.DDBLT_WAIT);

			blnBackBufferClear = true;
		}
		catch (Exception e) 
		{
			dixuBackBuffer.restore();
			dixuPrimarySurface.restore();
			MessageBox.Show(e.ToString());
		}
	}

	/// <summary>
	/// Create a surface from a bitmap file
	/// </summary>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <param name="FileName"></param>
	/// <param name="pic"></param>
	/// <returns></returns>
	public DirectDrawSurface7 dixuCreateSurface(int Width, int Height, string FileName,
		PictureBox pic) 
	{
		int PictureWidth, PictureHeight;
		DDSURFACEDESC2 ddsd = new DDSURFACEDESC2();
		DirectDrawSurface7 dds;
		string sBitMap;
		DDCOLORKEY key = new DDCOLORKEY();
		DDPIXELFORMAT pelFormat = new DDPIXELFORMAT();
			
		PictureWidth = pic.Width;
		PictureHeight = pic.Height;
				
		if(Width==0)
			Width = PictureWidth;
		if(Height==0)
			Height = PictureHeight;

		sBitMap = Application.StartupPath + "\\resources\\" + FileName + ".bmp";

		ddsd.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT |
			CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
		ddsd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
		ddsd.lWidth = Width;
		ddsd.lHeight = Height;
			
		dds = dixuDDraw.CreateSurfaceFromFile(sBitMap, ref ddsd);
		dds.GetPixelFormat(ref pelFormat); 
		key.low = pelFormat.lRBitMask + pelFormat.lBBitMask;
		key.high = key.low;

		dds.SetColorKey(CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref key);

		return dds;
		
			
	}
        
	/// <summary>
	/// Create a surface from a .resource file
	/// </summary>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <param name="ResourceName"></param>
	/// <param name="pic"></param>
	/// <returns></returns>
	public DirectDrawSurface7 dixuCreateSurfaceFromRes(int Width, int Height, string ResourceName,
		PictureBox pic) 
	{
		int PictureWidth, PictureHeight;
		DDSURFACEDESC2 ddsd = new DDSURFACEDESC2();
		DirectDrawSurface7 dds=null;
		DDCOLORKEY key = new DDCOLORKEY();
		DDPIXELFORMAT pelFormat = new DDPIXELFORMAT();
		const int SRCCOPY = 0x00CC0020;

		PictureWidth = pic.Width;
		PictureHeight = pic.Height;
				
		if(Width==0)
			Width = PictureWidth;
		if(Height==0)
			Height = PictureHeight;

			
		ddsd.lFlags = CONST_DDSURFACEDESCFLAGS.DDSD_CAPS | CONST_DDSURFACEDESCFLAGS.DDSD_HEIGHT |
			CONST_DDSURFACEDESCFLAGS.DDSD_WIDTH;
		ddsd.ddsCaps.lCaps = CONST_DDSURFACECAPSFLAGS.DDSCAPS_OFFSCREENPLAIN;
		ddsd.lWidth = Width;
		ddsd.lHeight = Height;

		//obtain the current culture settings
		CultureInfo ci = Application.CurrentCulture;
		//get a handle into the resource container
		ResourceManager rm = new ResourceManager("Ecalpon.ecalpon", Assembly.GetExecutingAssembly());
		//grab the bitmap
		Bitmap bm = (Bitmap)rm.GetObject("fighter", ci);
		//make a null IntPtr (pointer) .. can't assign null directly to it
		IntPtr hdc = (IntPtr)null;
		//convert the pointer to a device context
		IntPtr memDC = CreateCompatibleDC(hdc);
		//load the device context of the bitmap
		SelectObject(memDC, bm.GetHbitmap());
		//create a surface to hold the bitmap
		dds = dixuDDraw.CreateSurface(ref ddsd);
		//get the device context of the surface
		int hMemdc = dds.GetDC();
		//cast the handle into a pointer
		IntPtr ddsDC = (IntPtr)hMemdc;
		//copy the bitmap onto the surface
		BitBlt(ddsDC, 0, 0, bm.Width, bm.Height, memDC, 0, 0, SRCCOPY);
		//free the device context of the surface
		dds.ReleaseDC(hMemdc);
		//free the device context of the bitmap
		DeleteObject(memDC);

		dds.GetPixelFormat(ref pelFormat); 
		key.low = pelFormat.lRBitMask + pelFormat.lBBitMask;
		key.high = key.low;

		dds.SetColorKey(CONST_DDCKEYFLAGS.DDCKEY_SRCBLT, ref key);
	
		return dds;
			
	}


	/// <summary>
	/// Draw text onto a surface
	/// </summary>
	/// <param name="OutText"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void dixuDrawText(string OutText, int x, int y) 
	{	
		dixuBackBuffer.SetForeColor(ColorTranslator.ToOle(Color.Red));
		dixuBackBuffer.DrawText(x, y, OutText, false);
	}


}



