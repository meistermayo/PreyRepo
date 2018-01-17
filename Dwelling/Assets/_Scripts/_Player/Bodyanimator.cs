using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodyanimator : MonoBehaviour {
	[SerializeField] float maxHorDist;
	[SerializeField] float minHorDist;
	[SerializeField] float extremeHorDist;
	[SerializeField] float maxVerDist;

	[SerializeField] float horForce;
	[SerializeField] float verForce;

	[SerializeField] float tailSpeed;

	[SerializeField] Rigidbody2D hips;
	[SerializeField] Transform chest;
	[SerializeField] Transform hindLegs;
	Transform[] tailPieces;
	Vector3[] tailPiecesPreviousPosition;
	Transform[] waistPieces;
	[SerializeField] float whatAreYouFuckingDoing;
	// Update is called once per frame


	Vector3[] chestPreviousPosition;
	float side=1f;

	void Awake()
	{
		tailPieces = hips.transform.Find ("Tail").GetComponentsInChildren<Transform>();
		tailPiecesPreviousPosition = new Vector3[tailPieces.Length];
		for (int i = 0; i < tailPieces.Length; i++) {
			tailPiecesPreviousPosition[i]= tailPieces [i].position;
		}
		waistPieces = transform.Find ("Waist").GetComponentsInChildren<Transform> ();

		chestPreviousPosition = new Vector3[20];
		for (int i = 0; i < chestPreviousPosition.Length; i++) {
			chestPreviousPosition[i] = Vector3.zero;
		}
	}

	void FixedUpdate () {
		//CheckHorDist ();
		//CheckVerDist ();
		ParentHindLegs ();
		AnimateTail ();
		AnimateWaist ();
		AnimateHips ();
	}


	void CheckHorDist(){
		float diff = hips.position.x - transform.position.x;
		if (Mathf.Abs (diff) > extremeHorDist) {
			hips.position = new Vector2 (transform.position.x + Mathf.Sign (diff) * extremeHorDist, hips.position.y);
		}
		if (Mathf.Abs (diff) > maxHorDist) {
			hips.position = Vector2.Lerp (hips.position,new Vector2(transform.position.x + Mathf.Sign(-side) * maxHorDist,hips.position.y),horForce);
		}
	}

	void CheckVerDist(){
		float diff = hips.position.y - transform.position.y;
		if (Mathf.Abs (diff) > maxVerDist) {
			hips.position = new Vector2(hips.position.x,transform.position.y + Mathf.Sign(diff) * maxVerDist);
			hips.velocity = new Vector2 (hips.velocity.x, GetComponent<Rigidbody2D> ().velocity.y);
			//if (diff < 0.0f) 
			{
				hips.AddForce (Vector2.down * diff * verForce);
			}
		}
	}

	public void FlipChest(float h)
	{
		if ((h) != 0f) {
			chest.localScale = new Vector3 (Mathf.Sign (h) * Mathf.Abs (chest.localScale.x), chest.localScale.y);
			side = h;
		}
	}

	void ParentHindLegs()
	{
		hindLegs.position = hips.position + Vector2.down * 0.25f;
	}

	void AnimateTail()
	{	//kms
		float xDiff, yDiff; 
		{
			Vector3 vecDiff = tailPieces [tailPieces.Length - 1].position - tailPiecesPreviousPosition [tailPieces.Length - 1];
			xDiff = vecDiff.x;
			yDiff = vecDiff.y;
		}

		tailPieces [tailPieces.Length - 1].rotation = Quaternion.Euler(new Vector3(0f,0f,Mathf.Atan2(yDiff,xDiff)*Mathf.Rad2Deg + 90f) );

		for (int i = tailPieces.Length - 1; i > 0; i--) {
			tailPieces [i].position = Vector3.Lerp (tailPieces [i].position, tailPiecesPreviousPosition [i - 1],tailSpeed);
		}
		tailPieces [0].position = Vector3.Lerp (tailPieces[0].position, hips.position, tailSpeed);

		for (int i = 0; i < tailPieces.Length; i++) {
			tailPiecesPreviousPosition[i] = tailPieces [i].position + Vector3.right * -side * 0.1f;
		}
	}

	void AnimateWaist()
	{
		for (int i = 0; i < waistPieces.Length; i++) {
			waistPieces [i].localPosition = ((Vector3)(hips.transform.localPosition))*  (i*(0.5f));
		}
	}

	void AnimateHips()
	{
		bool flipX = (hips.position.x - chest.position.x > 0f);
		hips.GetComponent<SpriteRenderer>().flipX = flipX;
		hips.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer> ().flipX = flipX;
		hips.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer> ().flipX = flipX;
		hips.gravityScale = 0f;
		for (int i = 1; i < chestPreviousPosition.Length; i++) {
			chestPreviousPosition [i] = chestPreviousPosition [i-1];
		}
		chestPreviousPosition [0] = chest.position;
		//if (Vector3.Distance(new Vector3(hips.position.x,hips.position.y),new Vector3 (chest.position.x,chest.position.y)) > minHorDist)
		Vector3 newHipPos = chestPreviousPosition [chestPreviousPosition.Length - 1];
		hips.position = Vector3.Lerp(new Vector3(hips.position.x,hips.position.y),new Vector3(newHipPos.x - side * .33f,hips.position.y),minHorDist);
		hips.position = new Vector3 (hips.position.x, newHipPos.y);//newHipPos + (Vector3.right * extremeHorDist * -side);
	}
}
