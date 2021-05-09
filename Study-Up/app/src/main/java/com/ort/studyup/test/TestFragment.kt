package com.ort.studyup.test

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.lifecycle.Observer
import com.ort.studyup.R
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.INTERNAL_ERROR_CODE
import com.ort.studyup.common.models.ExamCard
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_test.*
import kotlinx.android.synthetic.main.fragment_test.view.*

class TestFragment : BaseFragment(){

    private val viewModel: TestViewModel by injectViewModel(TestViewModel::class)
    private lateinit var currentCard: ExamCard

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_test, container, false)
    }

    override fun onStart() {
        super.onStart()
        timer.start()
    }

    override fun onPause() {
        super.onPause()
        viewModel.sendAnswers()
        //TODO: navigate to result screen or something
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        val examId = requireActivity().intent.extras?.getInt(EXAM_ID_KEY)
        examId?.let {
            initViewModel(examId)
            initUI()
        } ?: run {
            requireActivity().finish()
        }
    }

    private fun initViewModel(deckId: Int) {
        viewModel.loadCards(deckId).observe(viewLifecycleOwner, Observer {
            it?.let {
                onNewCard(it)
            } ?: run {
                showError(INTERNAL_ERROR_CODE, getString(R.string.no_questions_error))
                requireActivity().finish()
            }
        })
    }

    private fun initUI() {
        falseButton.setOnClickListener {
            viewModel.onAnswer(false).observe(viewLifecycleOwner, Observer {
                onNewCard(it)
            })
        }
        trueButton.setOnClickListener {
            viewModel.onAnswer(true).observe(viewLifecycleOwner, Observer {
                onNewCard(it)
            })
        }
    }

    private fun onNewCard(card: ExamCard) {
        currentCard = card
        cardContent.text = currentCard.question
    }

}