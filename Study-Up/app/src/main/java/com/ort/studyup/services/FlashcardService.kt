package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface FlashcardService {

    @POST("api/flashcards")
    suspend fun createFlashcard(@Body data: NewFlashCardRequest): Response<Flashcard>

    @PUT("api/flashcards/{id}")
    suspend fun updateFlashcard(@Path("id") id: Int, @Body data: EditFlashCardRequest): Response<*>

    @DELETE("api/flashcards/{id}")
    suspend fun deleteFlashcard(@Path("id") id: Int): Response<*>

}