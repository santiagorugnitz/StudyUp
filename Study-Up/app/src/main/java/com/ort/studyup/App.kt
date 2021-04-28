package com.ort.studyup

import android.app.Application
import com.google.firebase.FirebaseApp

class App: Application() {

    override fun onCreate() {
        super.onCreate()
        KoinWrapper.start(this)
        FirebaseApp.initializeApp(this)
    }

}