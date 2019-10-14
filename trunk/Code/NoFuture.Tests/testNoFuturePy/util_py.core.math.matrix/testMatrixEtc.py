import unittest
import Util.Core.Math.Matrix.matrixEtc as toTest
import numpy as np

class TestMatrixEtc(unittest.TestCase):
    #Push-Location "C:\Projects\31g\trunk\Code\NoFuture.Tests\testNoFuturePy\util_py.core.math.matrix"
    #python -m unittest testMatrixEtc
    def test_dotProduct(self):
        a = [
            [0.838364753796889,0.937973900203581],
            [0.27903195483565,0.323496849892427],
            [0.977174767748068,0.358938428740454],
            [0.00173129513940369,0.584416931301549]
            ]
        b = [
            [0.310606089565254,0.5009241763041,0.607402099113633,0.520163896270173],
            [0.860155721129922,0.531705562272903,0.922568134461794,0.149769040825669]
            ]

        testResult = toTest.dotProduct(a,b)
        
        expected = [
                   [1.06720481433685,0.918683113743149,1.37456934256379,0.576566528183576],
                   [0.364926690557728,0.311778926604136,0.467932480396477,0.193592161729709],
                   [0.612259376446549,0.680340024714147,0.924683161720782,0.562048898716667],
                   [0.503227317797354,0.31160498065114,0.540216030360616,0.0884281204686199]
                   ]

        for i in range(4):
            for j in range(4):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)

    def test_dotProductWithNumpy(self):
        a = np.array([
                    0.930977594075248,
                    0.386082431946919,
                    0.73381193901124,
                    0.0183559898372535,
                    0.00742364302623255,
                    0.542181266724216,
                    0.323464508784686,
                    0.669235478466952
                    ])
        b = np.array([
                    [0.0336508625343679,0.344154772974623,0.797346453553693,0.177761182271764,0.560661230963031,0.68586849080672],
                    [0.935625466022466,0.746055849243913,0.974116698361056,0.600079462211616,0.194954827518647,0.457079552792515],
                    [0.362978033890472,0.0736942259006641,0.0669887787974387,0.0244248332569491,0.392087943103205,0.987495469389248],
                    [0.667282351603397,0.461360147903375,0.618741664857949,0.408348669488145,0.860615691105191,0.180829014713331],
                    [0.194897551645943,0.411182147176555,0.611133371298729,0.115583596804917,0.630847339812129,0.275854903401739],
                    [0.409338821381954,0.711846289556402,0.0431064851782781,0.417331045687818,0.393213809650957,0.585508570813345],
                    [0.324655027279935,0.543231854468226,0.209616580609985,0.694000578808598,0.295667964171464,0.206951371490467],
                    [0.296568319805231,0.232109084833464,0.835550106519624,0.741385492841427,0.56302459983296,0.480038081053662]
                    ])

        testResult = toTest.dotProduct(a,b)

        expected = [
                    [1.19803410815944,1.3910399160369,1.83380747257839,1.37036385191042,1.59105842160941,2.25065340898145]
                    ]

        for i in range(1):
            for j in range(6):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)
        

    def test_dotScalar(self):
        a = [
            [0.838364753796889,0.937973900203581],
            [0.27903195483565,0.323496849892427],
            [0.977174767748068,0.358938428740454],
            [0.00173129513940369,0.584416931301549]
            ]
        testResult = toTest.dotScalar(a, 1.112)
        expected = [
                   [0.93226160622214,1.04302697702638],
                   [0.310283533777242,0.359728497080378],
                   [1.08661834173585,0.399139532759385],
                   [0.0019252001950169,0.649871627607323]
                   ]
        for i in range(4):
            for j in range(2):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)

    def test_transpose(self):
        a = [
            [0.838364753796889,0.937973900203581],
            [0.27903195483565,0.323496849892427],
            [0.977174767748068,0.358938428740454],
            [0.00173129513940369,0.584416931301549]
            ]
        expected = [
                   [0.838364753796889,0.27903195483565,0.977174767748068,0.00173129513940369],
                   [0.937973900203581,0.323496849892427,0.358938428740454,0.584416931301549]
                   ]
        testResult = toTest.transpose(a)
        for i in range(2):
            for j in range(4):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)

    def test_crossProduct(self):
        a = [
            [0.838364753796889,0.937973900203581],
            [0.27903195483565,0.323496849892427],
            [0.977174767748068,0.358938428740454],
            [0.00173129513940369,0.584416931301549]
            ]
        expected = [
                   [1.73558781633467,1.2283875902536],
                   [1.2283875902536,1.45482519457203]
                   ]

        testResult = toTest.crossProduct(a)
        for i in range(2):
            for j in range(2):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)

    def test_innerProduct(self):
        a = [
            [0.838364753796889,0.937973900203581],
            [0.27903195483565,0.323496849892427],
            [0.977174767748068,0.358938428740454],
            [0.00173129513940369,0.584416931301549]
            ]
        expected = [
                    [1.58265049787204,0.537362158114426,1.15590376151827,0.549619285221219],
                    [0.537362158114426,0.182509043709727,0.38877843666369,0.189540122966995],
                    [1.15590376151827,0.38877843666369,1.08370732235016,0.211461472976446],
                    [0.549619285221219,0.189540122966995,0.211461472976446,0.34154614697478]
                    ]

        testResult = toTest.innerProduct(a)
        for i in range(4):
            for j in range(4):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.001, diff)

    def test_inverse(self):
        a = [
            [0.894001844590329,0.4769312498571252,0.5189747771279493,0.386991905245596],
            [0.5265429074382078,0.35963577333059893,0.9080173807305142,0.9165244453110933],
            [0.6110824363391651,0.2986528255669715,0.33236824373591245,0.24894181399022075],
            [0.6900334566811653,0.10749332673866197,0.39915603620358286,0.05989391997327187]
            ]

        expected = [
                    [-12.385650593256509,-0.2180428295818768,20.087324390144097,-0.1266717501323382],
                    [20.85065759378134,-1.0520510744014622,-28.25367802271775,-1.1901595620570176],
                    [18.742550887902343,0.48840430225867637,-31.77191975413348,3.481311260837718],
                    [-19.634640354450433,1.1452880686985094,31.023341122527462,-2.909217621114645]
                    ]
        testResult = toTest.inverse(a)
        for i in range(4):
            for j in range(4):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                #match tolerance of C# counterpart test
                self.assertGreater(0.00001, diff)

    def test_determinant(self):
        a = [
            [-0.39235803,0.67691051,-0.60700301,0.95378656],
            [-0.58099354,0.85133302,0.88056285,-1.96372078],
            [0.83324444,-0.08229884,-2.0713253,0.51832857],
            [-0.52886612,2.94227701,0.1462324,0.7010492]
            ]

        testResult = toTest.determinant(a)
        #match tolerance of C# counterpart test
        self.assertGreater(0.000001, (6.579618 - testResult))

    def test_selectMinor(self):
        a = [
            [2.0,1.0,13.0,4.0],
            [4.0,5.0,14.0,7.0],
            [7.0,12.0,9.0,10.0],
            [11.0,8.0,3.0,6.0]
            ]
        
        testResult = toTest.selectMinor(a, 0, 0)
        expected = [
                    [5,14,7],
                    [12,9,10],
                    [8,3,6]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        testResult = toTest.selectMinor(a, 1, 1)
        expected = [
                    [2,13,4],
                    [7,9,10],
                    [11,3,6]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        testResult = toTest.selectMinor(a, 2, 2)
        expected = [
                    [2,1,4],
                    [4,5,7],
                    [11,8,6]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        testResult = toTest.selectMinor(a, 3, 3)
        expected = [
                    [2,1,13],
                    [4,5,14],
                    [7,12,9]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        testResult = toTest.selectMinor(a, 0, 1)
        expected = [
                    [4,14,7],
                    [7,9,10],
                    [11,3,6]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)
        
        testResult = toTest.selectMinor(a, 0, 2)
        expected = [
                    [4,5,7],
                    [7,12,10],
                    [11,8,6]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        testResult = toTest.selectMinor(a, 0, 3)
        expected = [
                    [4,5,14],
                    [7,12,9],
                    [11,8,3]
                    ]
        for i in range(3):
            for j in range(3):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

    def test_getSoftmax(self):
        a = [
            [1.0,2.0,3.0,4.0,1.0,2.0,3.0],
            [6.0,2.0,1.0,4.0,1.0,3.0,1.0]
            ]

        testResult = toTest.getSoftmax(a)
        expected = [
                    [0.0236405430215914,0.0642616585104962,0.174681298595722,0.47483299974438,0.0236405430215914,0.0642616585104962,0.174681298595722],
                    [0.817225925108085,0.0149680149347914,0.00550642496965687,0.110599502042806,0.00550642496965687,0.0406872830053471,0.00550642496965687]
                    ]

        for i in range(2):
            for j in range(7):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        a = [
            [0.3551959799635189,0.17699255381545498,0.8213551357092245,0.38529920699314335,0.5960021066935483,0.871198147629929,0.22958585930319053,0.8156348246708918]
            ]

        testResult = toTest.getSoftmax(a)
        expected = [[0.10126056601448626, 0.08473202506121506, 0.16139523169312137, 0.1043551810797197, 0.12883122094923555, 0.16964350807282952, 0.089307630805428, 0.16047463632396453]]

        for i in range(1):
            for j in range(8):
                trij = testResult[i][j]
                expect = expected[i][j]
                diff = abs(trij - expect)
                self.assertGreater(0.00001, diff)

        