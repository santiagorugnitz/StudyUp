package com.ort.studyup.study

import android.os.Bundle
import android.speech.tts.TextToSpeech
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.ort.studyup.R
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.IS_OWNER_EXTRA
import com.ort.studyup.common.models.RatedFlashcard
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.CommentDialog
import kotlinx.android.synthetic.main.fragment_study.*
import java.util.*

class StudyFragment : BaseFragment(), TextToSpeech.OnInitListener, CommentDialog.Callback {

    private val viewModel: StudyViewModel by injectViewModel(StudyViewModel::class)
    private var showingQuestion = true
    private lateinit var currentCard: RatedFlashcard
    private lateinit var tts: TextToSpeech

    private var isOwner: Boolean = false

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_study, container, false)
    }

    override fun onPause() {
        super.onPause()
        viewModel.updateScore()
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        val deckId = requireActivity().intent.extras?.getInt(DECK_ID_KEY)
        isOwner = requireActivity().intent.getBooleanExtra(IS_OWNER_EXTRA,false)
        tts = TextToSpeech(requireContext(), this)
        deckId?.let {
            initViewModel(deckId)
            initUI()

        } ?: run {
            requireActivity().finish()
        }
    }

    private fun initViewModel(deckId: Int) {
        viewModel.loadFlashcards(deckId).observe(viewLifecycleOwner, {
            it?.let {
                onNewCard(it)
            } ?: run {
                showError(INTERNAL_ERROR_CODE, getString(R.string.no_flashcards_error))
                requireActivity().finish()
            }
        })
    }

    private fun initUI() {
        wrongButton.setOnClickListener {
            viewModel.onWrong().observe(viewLifecycleOwner, {
                onNewCard(it)
            })
        }
        correctButton.setOnClickListener {
            viewModel.onCorrect().observe(viewLifecycleOwner, {
                onNewCard(it)
            })
        }
        flipButton.setOnClickListener { onFlip() }
        if(!isOwner){
            commentButton.setOnClickListener { onComment() }
        }
    }

    private fun onTTS() {
        if (!tts.isSpeaking) {
            tts.speak(cardContent.text, TextToSpeech.QUEUE_FLUSH, null, null)
        }
    }

    private fun onComment() {
        CommentDialog(requireContext(), this).show()
    }

    private fun onNewCard(flashcard: RatedFlashcard) {
        currentCard = flashcard
        hideAnswerButtons()
        showingQuestion = true
        loadContent()
    }

    private fun hideAnswerButtons() {
        wrongButton.visibility = View.GONE
        correctButton.visibility = View.GONE
    }

    private fun onFlip() {
        showAnswerButtons()
        showingQuestion = !showingQuestion
        loadContent()
    }

    private fun showAnswerButtons() {
        wrongButton.visibility = View.VISIBLE
        correctButton.visibility = View.VISIBLE
    }

    private fun loadContent() {
        if (showingQuestion) {
            cardHeader.text = getString(R.string.question)
            cardContent.text = currentCard.question
        } else {
            cardHeader.text = getString(R.string.answer)
            cardContent.text = currentCard.answer
        }
    }

    override fun onInit(status: Int) {
        if (status == TextToSpeech.SUCCESS) {
            val result = tts.setLanguage(Locale.US)
            if (!(result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED)) {
                ttsButton.setOnClickListener { onTTS() }
            }
        }
    }

    override fun onComment(comment: String) {
        viewModel.comment(currentCard.id, comment)
    }
}