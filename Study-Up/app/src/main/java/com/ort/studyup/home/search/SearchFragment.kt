package com.ort.studyup.home.search

import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.EditorInfo
import android.widget.Toast
import androidx.navigation.fragment.findNavController
import com.ort.studyup.R
import com.ort.studyup.common.QR_EXTRA
import com.ort.studyup.common.SCAN_ACTIVITY_REQUEST_CODE
import com.ort.studyup.common.ui.BaseFragment
import kotlinx.android.synthetic.main.fragment_search.*

class SearchFragment : BaseFragment() {

    private val viewModel: SearchViewModel by injectViewModel(SearchViewModel::class)

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_search, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }


    private fun initUI() {


        searchBar.setOnNavigationItemSelectedListener {
            when (it.itemId) {
                R.id.searchGroups -> {
                    viewModel.searchUsers = false
                    onSearch(searchInput.text.toString())
                }
                R.id.searchUsers -> {
                    viewModel.searchUsers = true
                    onSearch(searchInput.text.toString())
                }
            }
            true
        }

        qrScan.setOnClickListener {
            val intent = Intent(context, ScanActivity::class.java)
            startActivityForResult(intent, SCAN_ACTIVITY_REQUEST_CODE)
        }

        searchInput.setOnEditorActionListener { textView, id, _ ->
            if (id == EditorInfo.IME_ACTION_SEARCH) {
                onSearch(textView.text.toString())
                true
            }
            false
        }

    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == SCAN_ACTIVITY_REQUEST_CODE) {
            data?.getStringExtra(QR_EXTRA)?.let {
                searchInput.setText(it)
                onSearch(it)
            }

        }
    }

    private fun onSearch(query: String) {
        Toast.makeText(requireContext(), query, Toast.LENGTH_LONG).show()
    }

}