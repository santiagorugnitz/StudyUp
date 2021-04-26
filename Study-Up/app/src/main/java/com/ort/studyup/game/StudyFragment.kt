package com.ort.studyup.game

import android.os.Bundle
import android.speech.tts.TextToSpeech
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.RatedFlashcard
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_study.*
import java.util.*

class StudyFragment : BaseFragment(), TextToSpeech.OnInitListener {

    private val viewModel: StudyViewModel by injectViewModel(StudyViewModel::class)
    private var showingQuestion = true
    private lateinit var currentCard: RatedFlashcard
    private val tts: TextToSpeech = TextToSpeech(requireContext(), this)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_study, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        val deckId = requireActivity().intent.extras?.getInt(DECK_ID_KEY)
        deckId?.let {
            initViewModel(deckId)
            initUI()

        } ?: run {
            requireActivity().finish()
        }
    }

    private fun initViewModel(deckId: Int) {
        viewModel.loadFlashcards(deckId).observe(viewLifecycleOwner, Observer {
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
            viewModel.onWrong().observe(viewLifecycleOwner, Observer {
                onNewCard(it)
            })
        }
        correctButton.setOnClickListener {
            viewModel.onCorrect().observe(viewLifecycleOwner, Observer {
                onNewCard(it)
            })
        }
        flipButton.setOnClickListener { onFlip() }
        commentButton.setOnClickListener { onComment() }
    }

    private fun onTTS() {
        if (!tts.isSpeaking) {
            tts.speak(cardContent.text, TextToSpeech.QUEUE_FLUSH, null, null)
        }
    }


    private fun onComment() {
        //TODO:
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
            if (result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                Toast.makeText(requireContext(), "Language not supported", Toast.LENGTH_SHORT).show();
            } else {
                ttsButton.setOnClickListener { onTTS() }
            }

        } else {
            Toast.makeText(requireContext(), "Init failed", Toast.LENGTH_SHORT).show();
        }
    }
}