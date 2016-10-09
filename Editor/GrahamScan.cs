using UnityEngine;
using System.Collections.Generic;

namespace DataUtility{
	public class GrahamScan {
		private class PointData{
			public float x;
			public float y;
			public float angle;

			public PointData(Vector2 point,Vector2 baseVector){
				this.x = point.x;
				this.y = point.y;

				if(!Mathf.Approximately(Vector2.Distance(point,Vector2.zero),0.0f)){
					var cos = (this.x*baseVector.x + this.y*baseVector.y)/(Vector2.Distance(point,Vector2.zero)*Vector2.Distance(baseVector,Vector2.zero));
					var tmp = Mathf.Acos(cos) * Mathf.Rad2Deg;

					var cross = baseVector.x*y - baseVector.y*x;

					if(cross < 0){
						tmp += 180;
					}
					this.angle = tmp;
				}else{
					this.angle = 0;
				}
				Debug.Log(this.angle);
			}

			public float GetNorm(){
				var tmp = new Vector2(this.x,this.y);
				return Vector2.Distance(Vector2.zero,tmp);
			}

		}

		private List<PointData> pointList = new List<PointData>(); 

		public GrahamScan(List<Vector2> list){
			foreach(var point in list){
				pointList.Add(new PointData(point,Vector2.right));
			}
			pointList.Sort((a,b) => {
				var diff = (a.angle - b.angle);

				var tmpVal = diff < 0 ? -1: 1;
				if(Mathf.Approximately(diff,0.0f)){
					tmpVal = (int)(a.GetNorm() - b.GetNorm());
				}
				return tmpVal;
			});
			var tmp = pointList[0];
			pointList.Add(tmp);
		}

		public List<Vector2> CreateDebugList(){
			var debugList = new List<Vector2>();
			foreach(var point in pointList){
				debugList.Add(new Vector2(point.x,point.y));
			}
			return debugList;
		}

		public List<Vector2> GetPoint(){
			var tmp = new List<PointData>();
			var result = new List<Vector2>();
			foreach(var point in pointList){
				tmp.Add(point);
				if(tmp.Count < 3){
					continue;
				}

				while(isClockwise(tmp[tmp.Count-3],tmp[tmp.Count-2],tmp[tmp.Count-1])){
					tmp.RemoveAt(tmp.Count-2);
					if(tmp.Count < 3){
						break;
					}
				}
			}
			tmp.Add(pointList[1]);
			while(isClockwise(tmp[tmp.Count-3],tmp[tmp.Count-2],tmp[tmp.Count-1])){
				tmp.RemoveAt(tmp.Count-2);
				if(tmp.Count-2-pointList.Count >= 0){
					tmp.RemoveAt(tmp.Count-2-pointList.Count);
				}
			}
			tmp.RemoveAt(tmp.Count - 1);
			if(Mathf.Approximately(tmp[tmp.Count-1].x,0.0f) && Mathf.Approximately(tmp[tmp.Count-1].y,0.0f)){
				tmp.RemoveAt(tmp.Count - 1);
			}

			foreach(var point in tmp){
				result.Add(new Vector2(point.x,point.y));
			}

			return result;
		}

		private bool isClockwise(PointData origin,PointData a, PointData b){
			var x = a.x - origin.x;
			var y = a.y - origin.y;
			var x2 = b.x - a.x;
			var y2 = b.y - a.y;

			var cross = x*y2 - y*x2;

			return (cross <= 0);
		}

	}
}
