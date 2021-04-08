package com.ort.studyup

import android.app.Application

class App: Application() {

    override fun onCreate() {
        super.onCreate()
        KoinWrapper.start(this)
    }

}