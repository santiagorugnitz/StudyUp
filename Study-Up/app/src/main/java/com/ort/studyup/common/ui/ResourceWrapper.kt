package com.ort.studyup.common.ui

import android.content.Context
import androidx.annotation.StringRes

class ResourceWrapper(private val context: Context) {

    fun getString(@StringRes resource:Int): String = context.getString(resource)

}