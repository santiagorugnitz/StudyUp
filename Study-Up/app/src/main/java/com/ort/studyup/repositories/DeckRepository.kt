package com.ort.studyup.repositories

import com.ort.studyup.common.models.Deck
import com.ort.studyup.common.models.DeckData
import com.ort.studyup.common.models.NewDeckRequest
import com.ort.studyup.services.DeckService
import com.ort.studyup.services.check

class DeckRepository(
    private val deckService: DeckService,
) {

    suspend fun decksFromUser(id:Int): List<Deck> {
        return deckService.decksFromUser(id).check()
    }

    suspend fun createDeck(deckData: DeckData):DeckData{
        return deckService.createDeck(
            NewDeckRequest(deckData.name,deckData.difficulty,deckData.subject,deckData.isHidden)
        ).check()
    }

    suspend fun updateDeck(deckData: DeckData){
        deckService.updateDeck(
            deckData.id,
            NewDeckRequest(deckData.name,deckData.difficulty,deckData.subject,deckData.isHidden)
        ).check()
    }


}