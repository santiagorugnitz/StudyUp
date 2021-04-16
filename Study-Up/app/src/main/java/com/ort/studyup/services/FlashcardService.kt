package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface FlashcardService {

    //TODO
    @POST("TBD")
    suspend fun createFlashcard(data: NewFlashCardRequest): Response<*>

    @PUT("TBD")
    suspend fun updateFlashcard(id: Int, data: EditFlashCardRequest): Response<*>

    @DELETE("TBD")
    suspend fun deleteFlashcard(id: Int): Response<*>

}