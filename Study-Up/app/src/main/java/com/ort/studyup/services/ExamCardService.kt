package com.ort.studyup.services

import com.ort.studyup.common.models.*
import retrofit2.Response
import retrofit2.http.*

interface ExamCardService {

    @POST("api/examcards")
    suspend fun createExam(@Body data: NewExamCardRequest): Response<ExamCard>

    @DELETE("api/examcards/{id}")
    suspend fun deleteExamCard(@Path("id") id: Int): Response<*>

    @PUT("api/examcards/{id}")
    suspend fun editExamCard(@Path("id") id: Int,@Body data:EditExamCardRequest):Response<*>
}