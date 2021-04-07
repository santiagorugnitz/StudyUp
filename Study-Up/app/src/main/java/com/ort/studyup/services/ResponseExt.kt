package com.ort.studyup.services

import retrofit2.Response

fun <T> Response<T>.check(): T {
    if (isSuccessful) {
        body()?.let {
            return it
        }
    }
    throw Exception() //TODO: custom error
}