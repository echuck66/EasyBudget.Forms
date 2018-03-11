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

namespace EasyBudget.Uwp
{
    public class FileAccessHelper
    {
        public static string GetLocalDocumentsFilePath(string filename)
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string docsFolder = System.IO.Path.Combine(personalFolder, "EasyBudgetDocs");

            if (!System.IO.Directory.Exists(docsFolder))
            {
                System.IO.Directory.CreateDirectory(docsFolder);
            }

            string docFilePath = System.IO.Path.Combine(docsFolder, filename);
            return docFilePath;
        }

        public static string GetDataFilePath(string filename)
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string dataFolder = System.IO.Path.Combine(personalFolder, "..", "Library", "Databases");
            string dataFolder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

            if (!System.IO.Directory.Exists(dataFolder))
            {
                System.IO.Directory.CreateDirectory(dataFolder);
            }
            string dbFilePath = System.IO.Path.Combine(dataFolder, filename);

            //if (System.IO.File.Exists(docsFilePath))
                //System.IO.File.Delete(docsFilePath);

            return dbFilePath;
        }
    }
}
