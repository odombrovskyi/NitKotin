package com.quitmojo.android

import android.app.Application
import com.quitmojo.android.background.ProgressRefreshScheduler

class QuitMojoApplication : Application() {
    override fun onCreate() {
        super.onCreate()
        ProgressRefreshScheduler.schedule(this)
    }
}