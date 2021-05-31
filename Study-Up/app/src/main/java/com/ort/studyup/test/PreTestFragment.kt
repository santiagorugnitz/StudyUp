package com.ort.studyup.test

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.ort.studyup.R
import com.ort.studyup.common.CORRECT_ANSWERS_KEY
import com.ort.studyup.common.EXAM_ID_KEY
import com.ort.studyup.common.SECONDS_KEY
import com.ort.studyup.common.TOTAL_KEY
import com.ort.studyup.common.models.Exam
import com.ort.studyup.common.renderers.EmptyViewRenderer
import com.ort.studyup.common.renderers.ResultItemRenderer
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.common.ui.ConfirmationDialog
import com.ort.studyup.common.ui.ScoreDialog
import com.thinkup.easylist.RendererAdapter
import kotlinx.android.synthetic.main.fragment_pre_test.*

class PreTestFragment : BaseFragment(), ConfirmationDialog.Callback {

    private val viewModel: PreTestViewModel by injectViewModel(PreTestViewModel::class)
    private val adapter = RendererAdapter()
    private var examId = 0
    private lateinit var dialog: ConfirmationDialog

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_pre_test, container, false)
    }

    override fun onStart() {
        super.onStart()
        dialog = ConfirmationDialog(
            requireContext(),
            getString(R.string.start_exam_confirmation),
            this,
        )
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        adapter.addRenderer(ResultItemRenderer())
        adapter.setEmptyItem(EmptyViewRenderer.Item(getString(R.string.no_results_yet)), EmptyViewRenderer())
        resultList.layoutManager = LinearLayoutManager(requireContext())
        resultList.adapter = adapter
        if (arguments?.containsKey(SECONDS_KEY) == true) {
            ScoreDialog(
                requireContext(),
                arguments?.getInt(CORRECT_ANSWERS_KEY) ?: 0,
                arguments?.getInt(TOTAL_KEY) ?: 0,
                arguments?.getInt(SECONDS_KEY) ?: 0
            ).show()
            startButton.visibility = View.GONE
        } else {
            startButton.visibility = View.VISIBLE
            startButton.setOnClickListener {
                dialog.show()
            }
        }
        examId = requireActivity().intent.extras?.getInt(EXAM_ID_KEY) ?: 0
        initViewModel(examId)
    }

    private fun initViewModel(examId: Int) {
        viewModel.loadExam(examId).observe(viewLifecycleOwner, {
            initUI(it.first)
            initResults(it.second)
        })
    }

    private fun initResults(list: List<Any>) {
        adapter.setItems(list)
    }

    private fun initUI(exam: Exam) {
        title.text = exam.name
        difficulty.text = resources.getStringArray(R.array.difficulties)[exam.difficulty]
        swipeRefresh.isEnabled = true
        swipeRefresh.setOnRefreshListener {
            swipeRefresh.isRefreshing = false
            viewModel.reloadResults(examId).observe(viewLifecycleOwner, {
                initResults(it)
            })
        }
    }

    override fun onButtonClick() {
        dialog.hide()
        findNavController().navigate(R.id.action_preTestFragment_to_testFragment)
    }


}