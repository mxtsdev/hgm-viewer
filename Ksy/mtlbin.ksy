meta:
  id: sltm
  file-extension: mtlbin
  endian: le
  encoding: ascii
seq:
  - id: header
    type: header_struct
  - id: unk1
    size: 16
    if: header.version == 3
  - id: mtl_name
    type: rec_struct
  - id: mtl
    type:
      switch-on: header.version
      cases:
        0: version0_struct
        1: version1_struct
        3: version3_struct
types:
  header_struct:
    seq:
      - id: magic
        contents: 'SLTM'
      - id: version
        type: u2
      - id: unk1
        type: u2
      - id: magic2
        contents: 'LTMB'
  version0_struct:
    seq:
    - id: unk1
      size: 17
    - id: textures
      type: textures0_struct
    - id: data
      type: material_data_struct
      size: 96
    - id: submtl
      type: rec0_struct
      repeat: eos
      if: not _io.eof
  version1_struct:
    seq:
    - id: unk1
      size: 17
    - id: textures
      type: textures_struct
    - id: data
      type: material_data_struct
      size: 96
    - id: submtl
      type: rec_struct
      repeat: eos
      if: not _io.eof
  version3_struct:
    seq:
    - id: unk1
      size: 1
    - id: textures
      type: textures_struct
    - id: data
      type: material_data_struct_3
      size: 197
    - id: submtl
      type: rec_struct
      repeat: eos
      if: not _io.eof
  material_data_struct:
    seq:
      - id: unk0
        size: 4
      - id: opacity
        type: u4
      - id: unk1
        size: 28
      - id: colors
        type: u4
      - id: unk2
        size: 16
      - id: u_size
        type: f4
      - id: v_size
        type: f4
      - id: unk3
        size: 8
      - id: stretch_factor1
        type: f4
      - id: stretch_factor2
        type: f4
      - id: unk4
        size: 16
  material_data_struct_3:
    seq:
      - id: unk_x0
        size: 3
      - id: colors
        type: u1
      - id: opacity
        type: u1
      - id: unk_x1
        size: 7
      - id: strs
        type: x_len_str
        repeat: expr
        repeat-expr: 10
      - id: unk2
        size: 16
      - id: u_size
        type: f4
      - id: v_size
        type: f4
      - id: unk3
        size: 8
      - id: stretch_factor1
        type: f4
      - id: stretch_factor2
        type: f4
      - id: unk4
        size: 16
  textures_struct:
    seq:
      - id: base_map
        type: rec_struct
      - id: rm_map
        type: rec_struct
      - id: normal_map
        type: rec_struct
      - id: ao_map
        type: rec_struct
      - id: si_map
        type: rec_struct
      - id: colorization_map
        type: rec_struct
      - id: special_map
        type: rec_struct
  textures0_struct:
    seq:
      - id: base_map
        type: rec0_struct
      - id: rm_map
        type: rec0_struct
      - id: normal_map
        type: rec0_struct
      - id: ao_map
        type: rec0_struct
      - id: si_map
        type: rec0_struct
      - id: colorization_map
        type: rec0_struct
      - id: special_map
        type: rec0_struct
  rec_struct:
    seq:
      - id: rec_type
        type: u1
      - id: data
        type:
          switch-on: rec_type
          cases:
            2: len_str
  rec0_struct:
    seq:
      - id: data
        type: len_str
  len_str:
    seq:
      - id: len
        type: u4
      - id: str
        type: str
        size: len
  x_len_str:
    seq:
      - id: unk0
        type: u4
      - id: len
        type: u4
      - id: str
        type: str
        size: len