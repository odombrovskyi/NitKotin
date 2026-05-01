package com.nitkotin.android

import android.app.Application
import com.nitkotin.android.background.ProgressRefreshScheduler

class NitKotinApplication : Application() {
    override fun onCreate() {
        super.onCreate()
        ProgressRefreshScheduler.schedule(this)
    }
}