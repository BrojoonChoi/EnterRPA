namespace New_RPA;

public partial class Form_Main : Form
{
    bool isAuto = false;
    string mPath = "";
    string mPython = "";

    public Form_Main()
    {
        InitializeComponent();
        ReadConfigure();
        
        if (isAuto)
            IO_NameSpace.IO.Instance().init(mPython, mPath);
        else
            IO_NameSpace.IO.Instance().init(mPython);
        
        IO_NameSpace.IO.Instance().webHelper.close();
        this.Close();

        if (Application.MessageLoop == true) 
        {
            Application.Exit();
        }
        else
        {
            Environment.Exit(1);
        }

    }

    private void ReadConfigure()
    {
        IO_ini ini = new IO_ini();
        string sConfig = "config.ini";
        FileInfo fi = new FileInfo(sConfig);

        if (fi.Exists)
        {
            ini.Load(sConfig);
            String sSectionProgram = "Configuration";
            isAuto = ini[sSectionProgram]["AutoRun"].ToBool();
            mPath = ini[sSectionProgram]["Path"].ToString();
            mPython = ini[sSectionProgram]["PythonPath"].ToString();
        }
        else
        {
            WriteConfigure();
        }
    }
    private void WriteConfigure() 
    {
        IO_ini ini = new IO_ini();
        String sSectionProgram = "Configuration";
        ini[sSectionProgram]["AutoRun"] = isAuto;
        ini[sSectionProgram]["Path"] = mPath;
        ini[sSectionProgram]["PythonPath"] = mPython;
        ini.Save("config.ini");
    }
}