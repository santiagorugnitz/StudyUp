package com.ort.studyup.home.exams.examcards

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.*
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import kotlinx.android.synthetic.main.fragment_new_flashcard.*
import kotlinx.android.synthetic.main.item_spinner.view.*

class NewExamCardFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: NewExamCardViewModel by injectViewModel(NewExamCardViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_new_exam_card, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        val examId = arguments?.getInt(EXAM_ID_KEY)
        initSpinner(answerInput, resources.getStringArray(R.array.exam_answer), getString(R.string.answer))
        arguments?.getInt(EXAM_CARD_ID_KEY)?.let { id ->
            if (id != 0) {
                viewModel.examCardId = id
                arguments?.getString(QUESTION_KEY)?.let {
                    questionInput.setText(it)
                }
                arguments?.getBoolean(ANSWER_KEY)?.let {
                    answerInput.spinner.setSelection(if (it) 0 else 1)
                }

                saveButton.text = getString(R.string.save_changes)
                header.text = getString(R.string.edit_examcard)
                deleteButton.visibility = View.VISIBLE
                deleteButton.setOnClickListener {
                    ConfirmationDialog(
                        requireContext(),
                        getString(R.string.delete_exam_card_confirmation),
                        this,
                    ).show()
                }
            }
        }

        saveButton.setOnClickListener {
            viewModel.sendData(
                examId ?: 0,
                questionInput.text.toString(),
                answerInput.spinner.selectedItemPosition == 0
            ).observe(viewLifecycleOwner, Observer {
                if (it > 0)
                    requireActivity().onBackPressed()
            })
        }

    }

    override fun onButtonClick() {
        viewModel.deleteExamCard().observe(viewLifecycleOwner, Observer {
            if (it) {
                requireActivity().onBackPressed()
                requireActivity().onBackPressed()
            }
        })
    }
}