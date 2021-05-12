package com.ort.studyup.home.profile

import android.content.Intent
import android.graphics.Point
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.WindowManager
import android.widget.Toast
import androidmads.library.qrgenearator.QRGContents
import androidmads.library.qrgenearator.QRGEncoder
import androidx.lifecycle.Observer
import androidx.navigation.fragment.findNavController
import com.google.zxing.WriterException
import com.ort.studyup.R
import com.ort.studyup.common.ui.BaseFragment
import com.ort.studyup.login.LoginActivity
import kotlinx.android.synthetic.main.fragment_profile.*

class ProfileFragment : BaseFragment() {

    private val viewModel: ProfileViewModel by injectViewModel(ProfileViewModel::class)

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        super.onCreate(savedInstanceState)
        return inflater.inflate(R.layout.fragment_profile, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        initUI()
    }

    private fun initUI() {
        viewModel.currentUser().observe(viewLifecycleOwner, Observer {
            it?.let {
                username.text = it.username
                loadQR(it.username)
                if (it.isStudent) {
                    rankingButton.visibility = View.VISIBLE
                    rankingButton.setOnClickListener {
                        findNavController().navigate(R.id.action_profileFragment_to_rankingFragment)
                    }
                } else {
                    rankingButton.visibility = View.GONE
                }
            }
        })
        logout.setOnClickListener {
            viewModel.logout()
            val intent = Intent(requireContext(), LoginActivity::class.java)
            startActivity(intent)
            requireActivity().finish()
        }
    }

    private fun loadQR(username: String) {
        val point = Point()
        requireActivity().getSystemService(WindowManager::class.java).defaultDisplay.getSize(point)

        val width = point.x
        val height = point.y

        var dimen = if (width < height) width else height
        dimen = dimen * 3 / 4

        val encoder = QRGEncoder(username, null, QRGContents.Type.TEXT, dimen)
        try {
            qrCode.setImageBitmap(encoder.encodeAsBitmap())
        } catch (e: WriterException) {
            Toast.makeText(requireContext(), e.message, Toast.LENGTH_SHORT).show()
        }

    }
}