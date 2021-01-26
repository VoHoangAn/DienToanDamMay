using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TestDockerWeb.Models;

namespace TestDockerWeb.MakeItWork
{
    static class DockerCommand
    {
        static DienToanDamMayEntities db = new DienToanDamMayEntities();

        static DockerCommand()
        {

        }
        private static string RunCommand(string command)
        {
            //try
            //{
            //    System.Diagnostics.ProcessStartInfo procStartInfo =
            //        new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            //    procStartInfo.RedirectStandardOutput = true;
            //    procStartInfo.UseShellExecute = false;
            //    procStartInfo.CreateNoWindow = true;

            //    System.Diagnostics.Process proc = new System.Diagnostics.Process();

            //    proc.StartInfo = procStartInfo;
            //    proc.Start();

            //    string result = proc.StandardOutput.ReadToEnd();
            //    return result;
            //}
            //catch
            //{
            //    return "";
            //}

            Process process = new Process();
            //test
            //process.StartInfo = new ProcessStartInfo("cmd.exe", command);
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            process.StandardInput.WriteLine(command);

            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();

            return process.StandardOutput.ReadToEnd();
        }

        public static void InitialContainer(string userName, string containerName, double cpu,
            string ram, string sshKey)
        {
            //CreateNeccessaryFolderWhenInitialContainer(containerName, sshKey);

            int getPort = 0;
            RunContainer(containerName, cpu, ram, ref getPort);

            string IP = GetContainerIP(containerName);

            ManageContainer newContainer = new ManageContainer
            {
                username = userName,
                containername = containerName,
                ip = IP,
                port = getPort,
                cpu = cpu,
                ram = ram,
                sshkey = sshKey,
                enddate = DateTime.Now.AddDays(3),
                status = "Đang chạy",
                sshcommand = "ssh root@" + IP + " -p " + getPort,
            };

            Feature.SaveContainer(newContainer);
        }

        private static void CreateNeccessaryFolderWhenInitialContainer(string containarName, string sshKey)
        {
            string sshCommand = @"ssh an@192.168.1.59\";

            //tự tạo thư mục cha nếu chưa có 
            string createDataFolder = "mkdir -p user/" + containarName + "/data\\";
            string createSshFolder = "mkdir -p user/" + containarName + "/.ssh/authorized_keys\\";

            //append dữ liệu vào file
            string writePublicKeyToSshFolder = "echo '" + sshKey + "' >> /.ssh/authorized_keys";

            RunCommand(sshCommand + createDataFolder + createSshFolder + writePublicKeyToSshFolder);
        }

        private static int ChoosePort()
        {
            int choosePort = 7000;

            List<int> portBeenUsed = db.ManageContainer.Select(mc => mc.port).ToList();

            while (choosePort < 10000)
            {
                if (portBeenUsed.Any(lp => lp == choosePort) == false)
                {
                    break;
                }
                choosePort++;
            }

            return choosePort;
        }

        public static void RunContainer(string containerName, double cpu, string ram, ref int port)
        {
            int getPort = ChoosePort();
            port = getPort;

            //string command = $"sudo docker run -t -d --cpus='{cpu}' --memory='{ram}' --net=custome-network " +
            //    $"-p {port}:22 --name={containerName} -v /user/{containerName}/data:/home/data " +
            //    $"-v /user/{containerName}/.ssh/authorized_keys:/.ssh/authorized_keys ubuntu-ssh-server";

            string test = @"cd \Users\USER\ && ssh an@192.168.1.59";
            string result = RunCommand(test);
            string openssh = "ssh an@192.168.1.59";
            string result1 = RunCommand(openssh);
            string password = "1";
            string result2 = RunCommand(password);

            string command = $"sudo docker run -t -d --cpus='{cpu}' --memory='{ram}' --net=custome-network " +
                $"-p {port}:22 --name={containerName} -v /user/{containerName}/data:/home/data " +
                $"-v /user/{containerName}/.ssh/authorized_keys:/.ssh/authorized_keys ubuntu-ssh-server";

            RunCommand(command);
        }

        public static string GetContainerIP(string containerName)
        {
            string openssh = "ssh an@192.168.1.59";
            RunCommand(openssh);
            string password = "1";
            RunCommand(password);

            string command = $"sudo docker inspect -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' {containerName}";

            return RunCommand(command);
        }

        public static void StopContainer(string containerName)
        {
            string openssh = "ssh an@192.168.1.59";
            RunCommand(openssh);
            string password = "1";
            RunCommand(password);

            string command = $"sudo docker stop {containerName} && " +
                $"sudo docker rm {containerName}";

            RunCommand(command);

            Feature.RemoveContainer(containerName);
        }

    }
}