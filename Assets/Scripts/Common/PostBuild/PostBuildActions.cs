﻿using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
#endif

namespace ARWT.Core{
    public class PostBuildActions{
        #if UNITY_EDITOR

        const string markerPlaceholder = "--MARKERS--";
        const string buildPlaceholder = "%UNITY_WEBGL_BUILD_URL%";
        static string projectMarkers = "Assets/Resources/Markers";
        static string projectImageMarkers = "Assets/Resources/ImageMarkers";
        static string projectMarkersImages = "Assets/Resources/Images";
        // 
        static string markersPath = "data/markers/";
        static string imagemarkersPath = "data/imagemarkers/";
        static string markersImagesPath = "data/markersImages/";
        static string imageMarkersImagesPath = "data/imageMarkersImages/";
        static string index = "index.html";
        static string appJS = "js/app.js";

        // static bool isImageTracking = true;
        static bool isImageTracking = false;

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath){
            Debug.Log($"targetPath: {targetPath}");
            var path = Path.Combine(targetPath, "Build/UnityLoader.js");
            var text = File.ReadAllText(path);
            text = text.Replace("UnityLoader.SystemInfo.mobile", "false");
            File.WriteAllText(path, text);

            string buildJsonPath = "Build/" + getName(targetPath) + ".json";
            path = Path.Combine(targetPath, "Build/" + getName(targetPath) + ".json");
            replaceInFile(path, "backgroundColor", "");

            generateHTML(targetPath);
            copyImages(targetPath);

            string unityDeclaration = $"const unityInstance = UnityLoader.instantiate(\"unityContainer\", \"{buildJsonPath}\");";
            replaceInFile(Path.Combine(targetPath, appJS), buildPlaceholder, unityDeclaration);
        }

        static string getName(string p){
            string[] pieces = new string[1];
            if(p.Contains("/")){
                pieces = p.Split('/');
            }else if(p.Contains("\\")){
                pieces = p.Split('\\');
            }
            return pieces[pieces.Length-1]; 
        }

        static void generateHTML(string targetPath){
            string html = "";
            string project;

            if (isImageTracking == false) {
                project = projectMarkers;
            } else{
                project = projectImageMarkers;
            }
            // var info = new DirectoryInfo(projectMarkers);

            var info = new DirectoryInfo(project);
            var fileInfo = info.GetFiles();
            string path;
            if (isImageTracking == false) {
                path = markersPath;
            } else {
                path = imagemarkersPath;
            }

            // var completeMarkersPath = Path.Combine(targetPath, markersPath);
            // var completeMarkersPath = Path.Combine(targetPath, imagemarkersPath);
            var completeMarkersPath = Path.Combine(targetPath + "/", path);
            Debug.Log("completeMarkersPath:" + completeMarkersPath);

            if(Directory.Exists(completeMarkersPath)){
                Directory.Delete(completeMarkersPath, true);
            }
            Directory.CreateDirectory(completeMarkersPath);

            foreach (var file in fileInfo){
                if (isImageTracking == false) {
                    if(file.Extension == ".patt"){
                        FileUtil.CopyFileOrDirectory(file.FullName, Path.Combine(completeMarkersPath, file.Name));
                        var markerName = file.Name.Replace(file.Extension, "");
                        var markerUrl = Path.Combine(path, file.Name);
                        html += $"\t\t\t<a-marker type=\"pattern\" url=\"{markerUrl}\" markercontroller=\"name : {markerName}\"></a-marker>\n";
                    }
                } else {
                    if(file.Extension != ".meta"){
                        FileUtil.CopyFileOrDirectory(file.FullName, Path.Combine(completeMarkersPath, file.Name));
                        // var markerName = file.Name.Replace(file.Extension, "");
                        // var markerUrl = Path.Combine(imagemarkersPath, file.Name);
                    }
                }
            }

            string host = "https://fushikky.github.io/nenga-tiger-ar/";
            // string host = "";
            imagemarkersPath =  host + imagemarkersPath;
            if (isImageTracking == true) {
                html += $"\t\t\t<a-nft type='nft' url='{imagemarkersPath}/trex'";
                html += "smooth='true' smoothCount='10' smoothTolerance='0.01' smoothThreshold='5' markercontroller='name : kite'>\n";
                html += "<a-box scale='50 50' position='50 150 -100'></a-box>\n";
                html += "</a-nft>\n";
            }

            replaceInFile(Path.Combine(targetPath, index), markerPlaceholder, html);
        }

        static void replaceInFile(string indexPath, string lookingFor, string replace){
            string[] lines = File.ReadAllLines(indexPath);
            List<string> newLines = new List<string>();
            foreach(var l in lines){
                if(l.Contains(lookingFor)){
                    newLines.Add(replace);
                }else{
                    newLines.Add(l);
                }
            }
            File.WriteAllLines(indexPath, newLines.ToArray());
        }

        static void copyImages(string targetPath){
            var info = new DirectoryInfo(projectMarkersImages);
            var fileInfo = info.GetFiles();
            var completeImagesPath = Path.Combine(targetPath, imageMarkersImagesPath);
            if(Directory.Exists(completeImagesPath)){
                Directory.Delete(completeImagesPath, true);
            }
            Directory.CreateDirectory(completeImagesPath);

            foreach (var file in fileInfo){
                if(file.Extension == ".jpg"){
                    FileUtil.CopyFileOrDirectory(file.FullName, Path.Combine(completeImagesPath, file.Name));
                }
            }
        }

        #endif
    }
}