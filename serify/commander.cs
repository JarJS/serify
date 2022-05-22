using System;
using System.IO;
using System.Net;
using Ionic.Zip;
using Newtonsoft.Json;

namespace Serify
{
    public static class Commander
    {
        public static int GLOBAL_MODULE = 400755;
        public static int LOCAL_MODULE = 400756;
        public static string path = Directory.GetCurrentDirectory();

        public static void ShowCommands()
        {
            Console.WriteLine("<command> [args..]\n");
            Console.WriteLine("Commands: ");
            Console.WriteLine("    help: Show all commands");
            Console.WriteLine("    new or n: Create a new project");
            Console.WriteLine("    add or a [global] <Module>: Add a module in your project");
        }

        private static void ExtractZIP(string local, string extracLocation)
        {
            if (File.Exists(local))
            {
                using(ZipFile zip = new ZipFile(local))
                {
                    if (Directory.Exists(extracLocation))
                    {
                        try
                        {
                            zip.ExtractAll(extracLocation);
                        }catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Consolo.Error("Dont have a folder to extract");
                    }
                }
            }else
            {
                Consolo.Error("Dont have a file");
            }
        }

        private static bool ProjectStarted()
        {
            string path = Directory.GetCurrentDirectory();
            if (File.Exists(path + "\\.jarpj"))
            {
                return true;
            }
            return false;
        }

        private static string Write(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        public static void CreateProject()
        {
            if (ProjectStarted())
            {
                Console.WriteLine("Project Exist");
                return;
            }else
            {
                try
                {
                    string name = Write("Project Name: ");
                    string description = Write("Project Description: ");
                    string version = Write("Project Version:(1.0.0) ");
                    string autor = Write("Autor: ");
                    string git = Write("Git Repository: ");
                    string license = Write("License:(MIT) ");

                    if(version == "")
                        version = "1.0.0";

                    if (license == "")
                        license = "MIT";

                    if(name != "")
                    {
                        string text = "{\n" +
                            "   \"name\": \"" + name + "\",\n" +
                            "   \"description\": \"" + description + "\",\n" +
                            "   \"version\": \"" + version + "\",\n" +
                            "   \"autor\": \"" + autor + "\",\n" +
                            "   \"git\": \"" + git + "\",\n" +
                            "   \"license\": \"" + license + "\",\n" +
                            "   \"frameworks\": {}\n" +
                            "}";
                        FileStream fs = File.Create(path + "\\.jarpj");
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(text);
                        sw.Close();
                    }
                    Consolo.Write("Project Created Successfuly!");
                }
                catch (Exception ex)
                {
                    Consolo.Error("Cant Create a New Project\n");
                    Consolo.Error(ex.Message);
                }
            }
        }

        public static dynamic GetJson(string caminhoArquivo)
        {
            string texto = File.ReadAllText(caminhoArquivo);
            return JsonConvert.DeserializeObject(texto);
        }

        public static void SaveJson(string file, dynamic content, bool ident)
        {
            Formatting format = ident ? Formatting.Indented : Formatting.None;
            string text = JsonConvert.SerializeObject(content, format);
            File.WriteAllText(file, text);
        }

        public static void AddModule(int type, string module)
        {
            if(type == GLOBAL_MODULE)
            {
            }
            else if(type == LOCAL_MODULE)
            {
                Console.WriteLine("Instaling...");
                if (!ProjectStarted())
                {
                    Consolo.Write("Project not started.\nYou want start a new project?(yes/no): ");
                    string response = Console.ReadLine();
                    if(response == "yes" || response == "y")
                        CreateProject();
                    else
                        Console.WriteLine("Project Dont Created.");
                        return;
                }

                if(!Directory.Exists(path+"\\_jarjs\\@modules"))
                    Directory.CreateDirectory(path + "\\_jarjs\\@modules");

                if (!Directory.Exists(path + "\\_jarjs\\@modules_temp"))
                    Directory.CreateDirectory(path + "\\_jarjs\\@modules_temp");

                if (!Directory.Exists(path + "\\_jarjs\\@modules\\" + module))
                    Directory.CreateDirectory(path + "\\_jarjs\\@modules\\" + module);

                System.Net.WebClient client = new System.Net.WebClient();
                string url = @"https://jarjs.vercel.app/@modules/" + module + ".zip";
                string local = path + "\\_jarjs\\@modules_temp\\" + module + ".zip";
                string finalPath = path + "\\_jarjs\\@modules\\" + module;
                try
                {
                    client.DownloadFile(url, local);
                }
                catch (WebException ex)
                {
                    Consolo.Error(ex.Message);
                    return;
                }

                ExtractZIP(local, finalPath);

                dynamic moduleObj = GetJson(finalPath + "\\_jarjs.json");
                if (moduleObj["frameworks"])
                {
                    foreach(var moduleEmbbebed in moduleObj["frameworks"])
                    {
                        AddModule(LOCAL_MODULE, moduleEmbbebed["name"]);
                    }
                }

                // Add module to framework in .jarpj

                dynamic project = GetJson(path + "\\.jarpj");

                project["frameworks"][module] = moduleObj["version"];
                project["runners"]["run"] = "serify run-this-dir %*";

                SaveJson(path + "\\.jarpj", project, true);

                Consolo.Success("Module(s) Added Successsfuly");
            }
        }
    }
}
