package com.ort.studyup.services

import android.content.Context
import android.media.session.MediaSession
import com.google.gson.FieldNamingPolicy
import com.google.gson.GsonBuilder
import com.ort.studyup.services.interceptors.TokenInterceptor
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class ServiceFactory(private val context: Context) {

    private val okHttpClient: OkHttpClient by lazy { okHttpClient() }

    fun <T> createInstance(clazz: Class<T>): T {

        return retrofit().create(clazz)
    }


    private fun okHttpClient(): OkHttpClient {
        val clientBuilder = OkHttpClient.Builder()
        clientBuilder.apply {
            addInterceptor(
                    HttpLoggingInterceptor().setLevel(HttpLoggingInterceptor.Level.BODY)
            )
            addInterceptor(TokenInterceptor(context))
        }
        return clientBuilder.build()
    }

    private fun retrofit() = Retrofit.Builder()
            //TODO: add url to BuildConfig
            .baseUrl("")
            .client(okHttpClient)
            .addConverterFactory(gsonConverterFactory())
            .build()

    private fun gsonConverterFactory() = GsonConverterFactory.create(
            GsonBuilder()
                    .setFieldNamingPolicy(FieldNamingPolicy.LOWER_CASE_WITH_UNDERSCORES)
                    .create()
    )
}