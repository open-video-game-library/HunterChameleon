# Hunter Chameleon (version 2.1.0)

[日本語版README](https://github.com/open-video-game-library/HunterChameleon/blob/main/README.md)

Hunter Chameleon is a 2D shoot 'em up game of chameleon.

Shoot down some targets that constantly appear with the chameleon's tongue.

![The image of Hunter Chameleon](https://user-images.githubusercontent.com/52689532/223369450-5b382679-08e8-43c9-a6de-0350c3860375.png)


## Contents

### Rule

Aim at the targets that appear (flies and apples) and shoot down them with chameleon's tongue.

If an apple hits the chameleon, the score go down.

### How to operate

Hunter Chamaleon is played with a mouse or gamepads.

- Mouse
   - Move the mouse: Move the reticle
   - Click the left mouse button: Shoot

- Gamepads
   - Left stick or D-Pad: Move the reticle
   - Right south button: Shoot

![Hunter-Chameleonの操作方法](https://user-images.githubusercontent.com/52689532/224528222-04e48fff-d515-4ee6-a2ee-ff96f1a119fc.png)


## Features

### Parameter Adjustment Function

The following parameters can be adjusted on the game.

- Time limit（second）
   - Default: 30

- Chameleon's tongue speed
   - Default: 8

- Color of chameleon's tongue and the reticle(RGBA)

   Default
   - Red: 243
   - Green: 132
   - Blue: 229
   - Alpha: 255

- Frequency of appearance of each target(per 10 second)

   Default
   - Fly: 10
   - Apple: 5

- Speed of each target

   Default
   - Fly: 5
   - Apple(gravity scale): 0.5

- Gamepad sensitivity
   - Default: 5
   
   Please check [here](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/SupportedDevices.html#gamepads) for supported gamepads.


### Parameter Output Function

The following parameters can be outputted as a CSV file at the end of the game.

- Score
- Number of hits
- Hit ratio

### Research Applications

1. Comparative evaluation of pointing performance of multiple game controllers

   Researcher ask participants to play this game with multiple game devices, and compare the hit rate obtained by the parameter output function to make a relative evaluation.
   
2. Evaluation of the experience of the mouse-operated system

   Researcher evaluate qualitatively by interviewing the players after they experience the mouse-operated system.
   
For specific case studies using Hunter-Chameleon, please see [this paper](http://id.nii.ac.jp/1001/00214482/).


## Requirement

MacOS, Unity 2021.3.19f1


## Installation

Data in this repository can be cloned to the local environment by entering the following command.

```bash
git clone git@github.com:open-video-game-library/HunterChameleon.git
```

After the code is clone and Unity is opened in the above requirement, import the following packages.

- Input System (version 1.5.0)
- Universal RP (version 12.1.10)


<img width="798" alt="Imported packages" src="https://user-images.githubusercontent.com/52689532/224526382-22179650-5c02-403d-9e13-ac8ff0b16bd5.png">


## Usage

All the data necessary for Hunter Chameleon to work is included.

If you want to change the graphic, background music, or other assets, import and replace them each time.

Some of the assets included in the project also include materials provided by outside parties that are permitted for redistribution. If you wish to publish game data containing such assets, please check the license file included with each asset.


## License

This content is licensed under the [MIT License](https://github.com/open-video-game-library/HunterChameleon/blob/main/LICENSE).

No sound files such as sound effect and BGM are included in this code because of redistribution.


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

If you have any comments, requests or questions, please contact us [here](https://openvideogame.cc/contact/).
