# RubikEnc
Provably secure encryption schemes with zero-setup and linear speed by using Rubik cubes
## Unity Project

To show how the encryption scheme works with 3D implementation and provides six orthogonal views of the cube's faces

- **Functionality**:
    - **Encode: initialize all arrows on the rubik, you can input with**
        - 108 bits, map to 54 arrows: '00' -> '↑', '01' -> '→', '10' -> '↓', '11' -> '←'.
        - string 'random': initialize rubik randomly.
        - string 'u' or 'up': initialize rubik with all upward arrows.
        - string 'r' or 'right': initialize rubik with all rightward arrows.
        - string 'd' or 'down': initialize rubik with all downward arrows.
        - string 'l' or 'left': initialize rubik with all leftward arrows.
    - **Rotate**:
        - there are 12 basic rotate operations, uppercase means clockwise rotate, lowercase is the opposite.
        - 'U' and 'u': rotate **up face**.
        - 'L' and 'l': rotate **left face**.
        - 'F' and 'f': rotate **front face**.
        - 'R' and 'r': rotate **right face**.
        - 'D' and 'd': rotate **down face**.
        - 'B' and 'b': rotate **back face**.
    - **Decode**:
        - remap arrows to binary codes.

Press 'C' to hide/show the 3D rotate buttons

Press 'V' to color the rubik cube

## Cpp Project
```
// initialize rubik with 108 bits string
void Encode(std::string seq);

// rotate rucik with the specified rotation sequence
void Rotate(std::string seq);

// decode rubik to 108 bits string
std::string Decode();

```
