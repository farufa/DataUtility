using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;

namespace DataUtility{
	public class ColliderPointGenerator : EditorWindow {
	
		[MenuItem("DataUtility/ColliderPointEdit")]
		public static void GeneratePoints(){
			var path = EditorUtility.OpenFilePanel ("txtファイルを選択してください", "","txt");
			if(path == ""){
				return;
			}
			var text = readFile(path);
			var list = convertList(text);

			var graham = new GrahamScan(list);
			var newList = graham.GetPoint();

			debugPoint(list);
			debugPoint(newList);
		}

		[MenuItem("DataUtility/Debug ColliderPointEdit")]
		public static void DebugGeneratePoints(){
			var list = generateTest(30);
			debugPoint(list);
			
			var graham = new GrahamScan(list);
			debugPoint(graham.CreateDebugList());

			var newList = graham.GetPoint();
			debugPoint(newList);
		}

		private static List<Vector2> convertList(string text){
			var list = new List<Vector2>();
			var lineArray = text.Split('\n');

			foreach(var line in lineArray){
				var tmp = line.Split(',');
				if(tmp.Length == 2){
					list.Add(new Vector2(float.Parse(tmp[0]),float.Parse(tmp[1])));
				}
			}

			return list;
		}

		private static string readFile(string path){
			var fi = new FileInfo(path);
			string result;
			using(var sr = new StreamReader(fi.OpenRead(),Encoding.UTF8)){
				result = sr.ReadToEnd();
			}
			return result;
		}

		private static List<Vector2> generateTest(int num){
			var test = new List<Vector2>();
			test.Add(new Vector2(0.0f,0.0f));
			for (var i = 1; i < num; i++){
				var x = Random.Range(-2000f,2000f);
				var y = Random.Range(0.0f,6000f);
				test.Add(new Vector2(x,y));
			}
			return test;
		}

		private static void debugPoint(List<Vector2> list){
			var obj = new GameObject();
			var col = obj.AddComponent<PolygonCollider2D>();
			col.points = list.ToArray();
		}

	}
}
