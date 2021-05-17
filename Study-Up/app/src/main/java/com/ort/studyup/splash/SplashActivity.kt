package com.ort.studyup.splash

import android.content.Intent
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.util.Log
import android.widget.Toast
import com.ort.studyup.R
import com.ort.studyup.common.NOTIFICATION_BODY_EXTRA
import com.ort.studyup.common.NOTIFICATION_ENTITY_ID_EXTRA
import com.ort.studyup.common.NOTIFICATION_TITLE_EXTRA
import com.ort.studyup.common.NOTIFICATION_TYPE_EXTRA
import com.ort.studyup.common.models.Notification
import com.ort.studyup.common.ui.BaseActivity
import com.ort.studyup.home.StudentHomeActivity
import com.ort.studyup.home.TeacherHomeActivity
import com.ort.studyup.login.LoginActivity
import kotlinx.android.synthetic.main.activity_splash.*
import org.koin.android.viewmodel.ext.android.viewModel

class SplashActivity : BaseActivity() {

    private val viewModel: SplashViewModel by viewModel(SplashViewModel::class)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash)
        if (intent.hasExtra(NOTIFICATION_TYPE_EXTRA)) {
            viewModel.saveNotification(
                intent.getStringExtra(NOTIFICATION_TITLE_EXTRA),
                intent.getStringExtra(NOTIFICATION_BODY_EXTRA),
                intent.getStringExtra(NOTIFICATION_TYPE_EXTRA)?.toIntOrNull(),
                intent.getStringExtra(NOTIFICATION_ENTITY_ID_EXTRA)?.toIntOrNull(),
            )
        }
        initAnimation()
    }

    private fun initAnimation() {
        icon.animate()
            .alpha(1.0F)
            .scaleX(2.0F)
            .scaleY(2.0F)
            .setDuration(750)
            .start()
    }

    override fun onResume() {
        super.onResume()
        Handler(Looper.getMainLooper()).postDelayed({
            initViewModel()
        }, 900)
    }

    private fun initViewModel() {
        viewModel.getUser().observe(this, {
            it?.let {
                val intent = if (it.isStudent) {
                    Intent(this, StudentHomeActivity::class.java)
                } else {
                    Intent(this, TeacherHomeActivity::class.java)
                }
                startActivity(intent)
                this.finish()
            } ?: run {
                val intent = Intent(this, LoginActivity::class.java)
                startActivity(intent)
                this.finish()
            }
        })
    }

}