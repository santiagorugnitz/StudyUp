package com.ort.studyup.test

import android.os.Bundle
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseActivity
import com.ort.studyup.common.ui.ConfirmationDialog

class TestActivity : BaseActivity(), ConfirmationDialog.Callback {

    private lateinit var dialog: ConfirmationDialog

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_study)
        dialog = ConfirmationDialog(
                this,
                getString(R.string.end_test),
                this
        )
    }

    override fun onBackPressed() {
        dialog.show()
    }

    override fun onButtonClick() {
        dialog.hide()
        super.onBackPressed()
    }
}
