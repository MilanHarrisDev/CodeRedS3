using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;


[RequireComponent(typeof(CloudRecoBehaviour))]
public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler {

    public static SimpleCloudHandler Instance;

    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";

    public bool canGetData { get; private set; }

    public string currentId { get; private set; }

    public bool isScanning { get { return mIsScanning; } }

    [SerializeField]
    private Text t_metadata, t_scanStatus;
    [SerializeField]
    private Button b_restartScan;

    private void Start()
    {
        currentId = "";
        canGetData = false;

        Instance = this;

        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour)
            mCloudRecoBehaviour.RegisterEventHandler(this);

    }

    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnInitialized()
    {
        Debug.Log("Cloud Reco Initialized");
    }

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        // do something with the target metadata
        mTargetMetadata = targetSearchResult.MetaData;
		
        if (!NetworkInfoManager.Instance.DeviceIdExists(mTargetMetadata))
        {
            NetworkInfoManager.Instance.AddNetworkDevice(mTargetMetadata);
            currentId = mTargetMetadata;
        }

		// stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.TargetFinder.ClearTrackables(false);
        }
    }

    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    private void Update () {
        t_scanStatus.text = "status: " + (mIsScanning ? "Scanning" : "Not Scanning");
        t_metadata.text = "currentId: " + mTargetMetadata;

        b_restartScan.interactable = !mIsScanning;

        canGetData = (!string.IsNullOrEmpty(currentId) && NetworkInfoManager.Instance.DeviceIdExists(currentId) && !isScanning);
	}

    public void RestartScan(){
        mCloudRecoBehaviour.CloudRecoEnabled = true;
    }
}
