package com.ort.studyup.services.interceptors

import android.content.Context
import com.ort.studyup.common.TOKEN_HEADER
import com.ort.studyup.common.TOKEN_KEY
import com.ort.studyup.common.utils.EncryptedPreferencesHelper
import okhttp3.Interceptor
import okhttp3.Response

class TokenInterceptor(context:Context) : Interceptor {

    private val preferencesHelper= EncryptedPreferencesHelper(context)


    override fun intercept(chain: Interceptor.Chain): Response {
        val token = preferencesHelper.getString(TOKEN_KEY)

        val requestBuilder = chain.request().newBuilder()
        if (!token.isNullOrEmpty()) {
            requestBuilder.addHeader(TOKEN_HEADER, token)
        }
        return chain.proceed(requestBuilder.build())
    }
}