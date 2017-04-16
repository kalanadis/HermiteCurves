using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHndler : MonoBehaviour {

	public CurveTest CurveI;
	private bool mLock;

	private Vector3 mX;
	private Vector3 mTempX;

	private Vector3 mPo;
	private Vector3 mTempPo;

	public GameObject PointPre;
	public GameObject PointPreX;

	private List<Vector2> P_V;

	private Vector3 mPrevPo;
	private Vector3 mNextPo;

	void Start(){

		P_V = new List<Vector2>();
		mLock = false;
	
	}
		
	void Update(){

		mPo = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mPo position is captured in each Update() call!

		if(!mLock && Input.GetMouseButtonDown(0)){

			mLock = true;//this is to prevent calling the same Input.GetMouseButtonDown(0) twice!
			mX=mPo;
		}
			
		if(mLock){

			mTempX = mPo;

			//Controll points to draw the Curve will be only recorded if the distance from the last point is greater than 1 
			//1 is the sensitivity of the Curve

			if(Vector3.Distance(mTempX,mX)>1){ 

				mTempPo = new Vector3(mPo.x,mPo.y,0);
				GameObject.Instantiate(PointPre,mTempPo+new Vector3(0,0,0.01f),Quaternion.identity);
				P_V.Add(mTempPo);

				mX= mTempX;
			}
			else{

				GameObject.Instantiate(PointPreX,new Vector3(mPo.x,mPo.y,0.01f),Quaternion.identity);

			}

		}



		if(Input.GetMouseButtonUp(0)){

			//pre process the captured points to fit the Hermite Curve definition

			mLock = false;
						
			int i = 0;
			int j = P_V.Count;

			if(j<2)return;



			foreach(Vector2 vals in P_V){
			    
				//two consecutive values  CurveI.P_V list will be populated as control point position and control point velocity

				i++;
				CurveI.P_V.Add(vals);//control point positon

				if((i-2) < 0){
					
					mPrevPo = new Vector2(0,0);
					mNextPo = (P_V[i]-vals)*2;
				
				}
				else if(i==j){

					mPrevPo = new Vector2(0,0);
					mNextPo = (vals-P_V[i-2])*2;

				}
				else{

					mPrevPo = P_V[i-2];
					mNextPo = P_V[i];

				}

				CurveI.P_V.Add((mNextPo-mPrevPo));//control point velocity

			}
				
			CurveI.Run = true;//ask CurveTest Class to draw the curve

		}
			

	}
		
}
