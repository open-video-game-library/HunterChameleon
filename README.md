# Hunter Chameleon

[日本語版README](https://github.com/open-video-game-library/Hunter-Chameleon/blob/main/README.JP.md)

Hunter Chameleon is a 2D shoot 'em up game of chameleon.

Shoot down flies, apples, and other targets that constantly appear with the chameleon's tongue.

![The image of Hunter Chameleon](https://user-images.githubusercontent.com/52689532/222994483-b0638233-ecbb-4c2f-811d-4c572933444f.png)


## Contents

### Rule

Aim at the targets that appear (flies, apples, and bees) and shoot down them with chameleon's tongue.

If a bee or an apple hits the chameleon, it will be damaged.
The game is over when the chameleon is damaged three times.

### How to operate

Hunter Chamaleon is played with the mouse.
- Move the mouse: Move the sight
- Click the left mouse button: Shoot

![Hunter-Chameleonの操作方法](https://user-images.githubusercontent.com/52689532/196676762-4b561a4d-eacf-43a2-9de5-26b8e95a69aa.png)


## Features

### Parameter Adjustment Function

The following parameters can be adjusted on the game.

- Time limit（second）
   - Default: 30

- Frequency of appearance of each target(second/num)

   Default
   - Apple: 5
   - Bee: 8
   - Fly: 2

- Speed of each target

   Default
   - Apple: 0.50(gravity scale)
   - Bee: 0.05
   - Fly: 0.05

- Graphics for background and each target（only standalone）

- Sight color and shape（only standalone）


### Parameter Output Function

The following parameters can be outputted as a CSV file at the end of the game.

- Play time（second）
- Score
- Hit rate

### Research Applications

1. Comparative evaluation of pointing performance of multiple game controllers

   Researcher ask participants to play this game with multiple game devices, and compare the hit rate obtained by the parameter output function to make a relative evaluation.
   
2. Evaluation of the experience of the mouse-operated system

   Researcher evaluate qualitatively by interviewing the players after they experience the mouse-operated system.
   
For specific case studies using Hunter-Chameleon, please see [this paper](http://id.nii.ac.jp/1001/00214482/).


## Requirement

MacOS, Unity 2021.3.9f1


## Installation

Data in this repository can be cloned to the local environment by entering the following command.

```bash
git clone git@github.com:open-video-game-library/Hunter-Chameleon.git
```


## Usage

All the data necessary for Hunter Chameleon to work is included.

If you want to change the graphic, background music, or other assets, import and replace them each time.

Some of the assets included in the project also include materials provided by outside parties that are permitted for redistribution. If you wish to publish game data containing such assets, please check the license file included with each asset.


## License

This content is licensed under the [MIT License](https://github.com/open-video-game-library/Hunter-Chameleon/blob/main/LICENSE).


## Citation

研究利用しやすく標準性を目指したビデオゲームの設計と開発

Design and development of video games aimed at accessible to researchers and standardization.

[Click here for the paper page.](http://id.nii.ac.jp/1001/00212465/)

```
@inproceedings{weko_212571_1,
   author	 = "拓也,岡 and 拓也,川島 and 大智,林 and 恵太,渡邊",
   title	 = "研究利用しやすく標準性を目指したビデオゲームの設計と開発",
   booktitle	 = "エンタテインメントコンピューティングシンポジウム論文集",
   year 	 = "2021",
   volume	 = "2021",
   number	 = "",
   pages	 = "181--186",
   month	 = "aug"
}
```


## Contact

If you have any comments, requests or questions, please contact us [here](https://open-video-game-library.github.io/info/contact/).
