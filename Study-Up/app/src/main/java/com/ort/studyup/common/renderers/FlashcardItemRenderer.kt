package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_flashcard.view.*

class FlashcardItemRenderer(private val callback: Callback) : ViewRenderer<FlashcardItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_flashcard, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.question.text = model.question
        view.answer.text = model.answer
        view.setOnClickListener {
            callback.onEditFlashcard(model.id, model.question, model.answer)
        }

        if (model.showAnswer){
            view.answer.setBackgroundColor(view.resources.getColor(R.color.white))
        }
        else{
            view.answer.setBackgroundColor(view.resources.getColor(R.color.black))
        }

        view.answer.setOnClickListener {
            model.showAnswer = !model.showAnswer
            callback.onShowAnswerChanged()
        }
    }

    class Item(
            val id: Int,
            val question: String,
            val answer: String,
            var showAnswer:Boolean = false
    )

    interface Callback {
        fun onEditFlashcard(id: Int, question: String, answer: String)
        fun onShowAnswerChanged()
    }
}