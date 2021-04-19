package com.ort.studyup.home.search

import android.Manifest.permission.CAMERA
import android.Manifest.permission.VIBRATE
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Bundle
import android.widget.Toast
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import com.ort.studyup.R
import com.ort.studyup.common.PERMISSION_REQUEST_CODE
import com.ort.studyup.common.QR_EXTRA
import com.ort.studyup.common.ui.BaseActivity
import eu.livotov.labs.android.camview.ScannerLiveView
import eu.livotov.labs.android.camview.scanner.decoder.zxing.ZXDecoder
import kotlinx.android.synthetic.main.activity_scan.*


class ScanActivity : BaseActivity() {


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_scan)

        if (!checkPermissions()) {
            requestPermissions()
        }
        
        scanner.scannerViewEventListener = object : ScannerLiveView.ScannerViewEventListener {
            override fun onScannerStarted(scanner: ScannerLiveView?) {
            }

            override fun onScannerStopped(scanner: ScannerLiveView?) {
            }

            override fun onScannerError(err: Throwable?) {
                Toast.makeText(this@ScanActivity, err?.message, Toast.LENGTH_SHORT).show()
            }

            override fun onCodeScanned(data: String?) {
                data?.let {
                    val intent = Intent()
                    intent.putExtra(QR_EXTRA, it)
                    setResult(Activity.RESULT_OK, intent)
                    finish()
                }
            }
        }
    }

    override fun onResume() {
        super.onResume()
        val decoder = ZXDecoder()
        decoder.scanAreaPercent = 0.8
        scanner.decoder = decoder
        scanner.startScanner()
    }

    override fun onPause() {
        super.onPause()
        scanner.stopScanner()
    }

    private fun checkPermissions(): Boolean {
        return PackageManager.PERMISSION_GRANTED == ContextCompat.checkSelfPermission(applicationContext, CAMERA)
                && PackageManager.PERMISSION_GRANTED == ContextCompat.checkSelfPermission(applicationContext, VIBRATE)

    }

    private fun requestPermissions() {
        ActivityCompat.requestPermissions(this, arrayOf(CAMERA, VIBRATE), PERMISSION_REQUEST_CODE)
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        if (grantResults.size >= 2) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED && grantResults[1] == PackageManager.PERMISSION_GRANTED) {
                scanner.startScanner()
            } else {
                finish()
            }
        }
    }

}