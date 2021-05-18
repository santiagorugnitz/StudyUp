package com.ort.studyup.common.renderers

import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.ort.studyup.common.models.Comment
import com.thinkup.easycore.ViewRenderer
import kotlinx.android.synthetic.main.item_flashcard.view.*

class FlashcardItemRenderer(private val callback: Callback) : ViewRenderer<FlashcardItemRenderer.Item, View>(Item::class) {

    override fun create(parent: ViewGroup): View = inflate(R.layout.item_flashcard, parent, false)

    override fun bind(view: View, model: Item, position: Int) {
        view.question.text = model.question
        view.answer.text = model.answer
        view.editButton.setOnClickListener {
            callback.onEditFlashcard(model.id, model.question, model.answer)
        }

        setAnswerVisibility(view, model.showAnswer)

        view.answer.setOnClickListener {
            model.showAnswer = !model.showAnswer
            setAnswerVisibility(view, model.showAnswer)
        }
        if (model.comments.isEmpty()) {
            view.commentCountContainer.visibility = View.GONE
        } else {
            view.commentCountContainer.visibility = View.VISIBLE
            view.commentCount.text = if (model.comments.size > 9) "9+" else model.comments.size.toString()
            view.commentCountContainer.setOnClickListener { callback.onShowComments(model.id, model.comments) }
        }
    }

    private fun setAnswerVisibility(view: View, visible: Boolean) {
        if (visible) {
            view.answer.setBackgroundColor(view.resources.getColor(R.color.white))
        } else {
            view.answer.setBackgroundColor(view.resources.getColor(R.color.black))
        }
    }

    class Item(
        val id: Int,
        val question: String,
        val answer: String,
        val comments: MutableList<Comment>,
        var showAnswer: Boolean = false
    )

    interface Callback {
        fun onEditFlashcard(id: Int, question: String, answer: String)
        fun onShowComments(flashcardId: Int, comments: MutableList<Comment>)
    }
}