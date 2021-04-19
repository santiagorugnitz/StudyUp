package com.ort.studyup.services

import android.content.Context
import com.google.gson.FieldNamingPolicy
import com.google.gson.GsonBuilder
import com.ort.studyup.common.API_URL
import com.ort.studyup.services.interceptors.HeaderInterceptor
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.security.cert.CertificateException
import java.security.cert.X509Certificate
import javax.net.ssl.HostnameVerifier
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager

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
            addInterceptor(HeaderInterceptor(context))
        }

        acceptAllCertificates(clientBuilder)

        return clientBuilder.build()
    }

    private fun retrofit() = Retrofit.Builder()
        .baseUrl(API_URL)
        .client(okHttpClient)
        .addConverterFactory(gsonConverterFactory())
        .build()

    private fun gsonConverterFactory() = GsonConverterFactory.create(
        GsonBuilder()
            .setFieldNamingPolicy(FieldNamingPolicy.LOWER_CASE_WITH_UNDERSCORES)
            .setLenient()
            .create()
    )

    private fun acceptAllCertificates(clientBuilder: OkHttpClient.Builder): OkHttpClient.Builder {
        val sslContext = SSLContext.getInstance("SSL")
        val trustAllCerts = getTrustManagerX509()
        sslContext.init(null, trustAllCerts, java.security.SecureRandom())
        clientBuilder
            .sslSocketFactory(sslContext.socketFactory, trustAllCerts[0] as X509TrustManager)
            .hostnameVerifier(HostnameVerifier { _, _ -> true })
        return clientBuilder
    }

    private fun getTrustManagerX509(): Array<TrustManager> {
        return arrayOf(
            object : X509TrustManager {
                @Throws(CertificateException::class)
                override fun checkClientTrusted(chain: Array<X509Certificate>, authType: String) {
                }

                @Throws(CertificateException::class)
                override fun checkServerTrusted(chain: Array<X509Certificate>, authType: String) {
                }

                override fun getAcceptedIssuers(): Array<X509Certificate> {
                    return arrayOf()
                }
            }
        )
    }
}