package com.ort.studyup.home.flashcards

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.ANSWER_KEY
import com.ort.studyup.common.DECK_ID_KEY
import com.ort.studyup.common.FLASHCARD_ID_KEY
import com.ort.studyup.common.QUESTION_KEY
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_new_flashcard.*

class NewFlashcardFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: NewFlashcardViewModel by injectViewModel(NewFlashcardViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_flashcard, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        val deckId = arguments?.getInt(DECK_ID_KEY)

        arguments?.getInt(FLASHCARD_ID_KEY)?.let {
            if (it != 0) {
                viewModel.flashcardId = it
                arguments?.getString(QUESTION_KEY)?.let {
                    questionInput.setText(it)
                }
                arguments?.getString(ANSWER_KEY)?.let {
                    answerInput.setText(it)
                }

                saveButton.text = getString(R.string.save_changes)
                header.text = getString(R.string.edit_flashcard)
                deleteButton.visibility = View.VISIBLE
                deleteButton.setOnClickListener {
                    ConfirmationDialog(
                        requireContext(),
                        getString(R.string.delete_flashcard_confirmation),
                        this,
                        attrs = null
                    ).show()
                }
            }
        }

        saveButton.setOnClickListener {
            viewModel.sendData(
                deckId ?: 0, questionInput.text.toString(), answerInput.text.toString()
            ).observe(viewLifecycleOwner, Observer {
                if (it > 0)
                    requireActivity().onBackPressed()
            })
        }

    }

    override fun onButtonClick() {
        viewModel.deleteFlashcard().observe(viewLifecycleOwner, Observer {
            if (it) {
                requireActivity().onBackPressed()
                requireActivity().onBackPressed()
            }
        })
    }
}