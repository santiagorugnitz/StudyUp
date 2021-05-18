package com.ort.studyup.common.ui

import android.os.Bundle
import android.widget.FrameLayout
import androidx.annotation.CallSuper
import androidx.appcompat.app.AppCompatActivity
import com.ort.studyup.R

open class BaseActivity : AppCompatActivity() {

    @CallSuper
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onBackPressed() {
        try {
            val root = findViewById<FrameLayout>(android.R.id.content)
            val view = root?.findViewById<FrameLayout>(R.id.dialogContainer)
            if (view == null) super.onBackPressed()
            else
                root.removeView(view)
        } catch (e: Exception) {
            super.onBackPressed()
        }
    }

}