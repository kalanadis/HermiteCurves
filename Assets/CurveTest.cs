using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveTest : MonoBehaviour {

	public Material LineMat;

	public LineRenderer mLine;

	public List<Vector2> P_V;

	public int StepLimit;

	private int StepsCount;

	private float H0;
	private float H1;
	private float H2;
	private float H3;

	private float t1;
	private float t2;
	private float t3;

	private Vector2 mP0;
	private Vector2 mP1;

	private Vector2 mV0;
	private Vector2 mV1;

	private int CurveID;

	public bool Run;

	private Vector2 mTempPrevPoint;

	private int mLock;
	public int LockCap;

	private int mPointCount;

	void Start () {

		mLine.SetVertexCount (0);

		mPointCount = 0;

		mLock = 0;

		mTempPrevPoint = new Vector2 (-100,0);

		P_V.Clear();

		Run = false;

		StepsCount = 0;

		H0 = 0;
		H1 = 0;
		H2 = 0;
		H3 = 0;

		t1 = 0;
		t2 = 0;
		t3 = 0;
   
		CurveID = 0;

	}
				
	void FixedUpdate(){ //Draw the curve in FixedUpdate()

		if (!Run)
			return;

		mLock += 10;

		if(mLock<LockCap)return;

		mLock = 0;


		while(true){

			//Curve between two consecetive contrl points will be drawn in number of steps defined in "StepLimit" 

		if (StepLimit > StepsCount) {


			t1 = (float)StepsCount / StepLimit;
			        
			t2 = t1 * t1;

			t3 = t2 * t1;

			
			H0 = 1 - (3 * t2) + (2 * t3);
			H1 = t1 - (2 * t2) + t3;
			H2 = (-1 * t2) + t3;
			H3 = (3 * t2) - (2 * t3);

			mP0 = P_V [CurveID] * H0;
			mV0 = (P_V [CurveID + 1] * H1) * 0.5f;
			mP1 = P_V [CurveID + 2] * H3;
			mV1 = (P_V [CurveID + 3] * H2) * 0.5f;

			//mP0 postion according to t1 is calculated according to Hermite Curves
			mP0 = mP0 + mV0 + mV1 + mP1;


			//Displaying the curve is done using a line render 
			if (Vector2.Distance (mP0, mTempPrevPoint) > 0.25f) {
				
				mPointCount++;

				
				mTempPrevPoint = mP0;

					mLine.SetVertexCount (mPointCount);
					mLine.SetPosition (mPointCount - 1, mP0);
					LineMat.SetTextureScale("_MainTex",new Vector2(mPointCount-1,1));


				break;
			}
			   
			StepsCount++;
			

		} else {

			//move to next two control points
			CurveID += 2;
			StepsCount = 0;
			 
				if (CurveID >= (P_V.Count - 2)) {

					Run = false;
					break;
				}
			
		}
	 }


  }



}
