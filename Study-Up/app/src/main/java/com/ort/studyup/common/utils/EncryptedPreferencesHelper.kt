package com.ort.studyup.common.utils

import android.content.Context
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey

class EncryptedPreferencesHelper(
    context: Context,
) {

    private val FILE_NAME = "study-up.pref"
    private val masterKey = MasterKey.Builder(context).setKeyScheme(MasterKey.KeyScheme.AES256_GCM).build()
    private val preferences = EncryptedSharedPreferences.create(
        context,
        FILE_NAME,
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM)


    fun setString(key:String, value:String) = preferences.edit().putString(key,value).apply()

    fun getString(key:String, default:String?=null) = preferences.getString(key,default)

    fun clear(key:String) = preferences.edit().remove(key).apply()
}