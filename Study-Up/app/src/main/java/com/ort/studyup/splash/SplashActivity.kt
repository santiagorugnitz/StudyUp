package com.ort.studyup.splash

import android.content.Intent
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.models.NotificationType
import com.ort.studyup.common.ui.BaseActivity
import com.ort.studyup.home.StudentHomeActivity
import com.ort.studyup.home.TeacherHomeActivity
import com.ort.studyup.login.LoginActivity
import com.ort.studyup.storage.dao.Converters
import com.ort.studyup.study.StudyActivity
import com.ort.studyup.test.TestActivity
import kotlinx.android.synthetic.main.activity_splash.*
import org.koin.android.viewmodel.ext.android.viewModel

class SplashActivity : BaseActivity() {

    private val viewModel: SplashViewModel by viewModel(SplashViewModel::class)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash)
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
            if (startedFromNotification()) {
                customDestination()
            } else {
                defaultDestination()
            }
        }, 900)
    }

    private fun defaultDestination() {
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

    private fun customDestination() {
        val ordinal = intent.getStringExtra(NOTIFICATION_TYPE_EXTRA)?.toIntOrNull()
        val entityId = intent.getStringExtra(NOTIFICATION_ENTITY_ID_EXTRA)?.toIntOrNull()
        ordinal?.let {
            entityId?.let {
                when (Converters().toNotificationTypeEnum(ordinal)) {
                    NotificationType.DECK -> {
                        val intent = Intent(this, StudyActivity::class.java)
                        intent.putExtra(DECK_ID_KEY, entityId)
                        startActivity(intent)
                        this.finish()
                    }
                    NotificationType.EXAM -> {
                        val intent = Intent(this, TestActivity::class.java)
                        intent.putExtra(EXAM_ID_KEY, entityId)
                        startActivity(intent)
                        this.finish()
                    }
                    NotificationType.COMMENT -> {
                        val intent = Intent(this, StudentHomeActivity::class.java)
                        intent.putExtra(DECK_ID_KEY, entityId)
                        startActivity(intent)
                        this.finish()
                    }
                }
            }
        }
    }

    private fun startedFromNotification() = intent.hasExtra(NOTIFICATION_TYPE_EXTRA) && intent.hasExtra(NOTIFICATION_ENTITY_ID_EXTRA)

}