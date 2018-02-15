//
//  Copyright 2018  CrawfordNET Solutions, LLC
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.IO;
using Android.Content;

namespace EasyBudget.Droid
{
    public class FileAccessHelper
    {
        public static string GetLocalDocumentsPath(string filename)
        {
            string docsFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "EasyBudgetDocs");
            if (!System.IO.Directory.Exists(docsFolder))
            {
                System.IO.Directory.CreateDirectory(docsFolder);
            }
            string docsFilePath = System.IO.Path.Combine(docsFolder, filename);

            return docsFilePath;
        }

        public static string GetLocalDataPath(string filename)
        {
            string dataFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "EasyBudgetData");
            if (!System.IO.Directory.Exists(dataFolder))
            {
                System.IO.Directory.CreateDirectory(dataFolder);
            }
            string docsFilePath = System.IO.Path.Combine(dataFolder, filename);

            return docsFilePath;
        }


    }
}
