package com.ort.studyup.services

import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.NewDeckRequest
import retrofit2.Response
import retrofit2.http.*

interface DeckService {

    //TODO
    @GET("TBD")
    suspend fun decksFromUser(id: Int): Response<List<Deck>>

    @POST("TBD")
    suspend fun createDeck(data: NewDeckRequest): Response<DeckData>

    @PUT("TBD")
    suspend fun updateDeck(id: Int, data: NewDeckRequest): Response<*>

    @DELETE("TBD")
    suspend fun deleteDeck(id: Int): Response<*>

}