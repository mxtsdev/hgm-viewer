meta:
  id: hgm
  file-extension: hgm
  endian: le
  encoding: ascii
  #ks-debug: true
  # humanhead_male_08_mesh.hgm: only six unknown bytes before bones/joints. they must mean something
  # humanhead_male_15_mesh.hgm: completely broken armature with [0,0,0] translations on every bone
seq:
  - id: header
    type: hgm_header
    size: 32
  - id: num_meshes
    type: u4
  - id: meshes
    type: mesh
    repeat: expr
    repeat-expr: num_meshes
  - id: armature_element_type
    type: u1
  - id: armature
    type: hgm_armature
    if: armature_element_type == 0x01
types:
  hgm_header:
    seq:
      - id: magic
        contents: [ 0x68, 0x73, 0x6d, 0x68]
        size: 4 # unknown
      - id: version
        type: u2
      - size: 2 # unknown
      - id: bbox # theory
        type: bbox
  mesh:
    seq:
      - id: material_index
        type: u4
      - size: 8 # unknown
      - id: num_vertices
        type: u4
      - id: num_faces
        type: u4
      - id: have_vertex_type
        type: u1
        enum: vertex_type_marker
      - id: vertex_type
        type: len_str
        if: have_vertex_type == vertex_type_marker::set
      - id: fields_arr
        type: record_array
      - size: 4 # unknown
      - id: size_vertex
        type: u4
      - size: 1
      - id: vertices
        type: vertex
        repeat: expr
        repeat-expr: num_vertices
        size: size_vertex
      - id: faces
        type: faces_struct
      - id: bbox # theory
        doc: Exists only when Header.Version >= 12
        type: bbox
        if: _root.header.version >= 12
      - id: bsphere # theory
        doc: Exists only when Header.Version >= 12
        type: f4
        repeat: expr
        repeat-expr: 4
        if: _root.header.version >= 12
    instances:
      fields:
        value: fields_arr.elements
        #value: fields_arr.data.as<record_array>.elements
        #if: 'fields_type.type1 == record_type::array'
      num_fields:
        value: fields_arr.num_elements
        #value: fields_arr.data.as<record_array>.num_elements
        #if: 'fields_type.type1 == record_type::array'
    types:
      vertex:
        seq:
          - id: fields
            type:
              switch-on: _parent.fields[_index].element_value_str #_parent.fields[_index].data.as<record_element>.value.str
              cases:
                '"fmt_sint16_c1"': vertex_field_sint16(1)
                '"fmt_sint16_c2"': vertex_field_sint16(2)
                '"fmt_sint16_c3"': vertex_field_sint16(3)
                '"fmt_unorm8_c1"': vertex_field_unorm8(1)
                '"fmt_unorm8_c2"': vertex_field_unorm8(2)
                '"fmt_unorm8_c3"': vertex_field_unorm8(3)
                '"fmt_unorm8_c4"': vertex_field_unorm8(4)
                '"fmt_uint8_c4"': vertex_field_uint8(4)
                _: vertex_field_unknown(_index)
            repeat: expr
            repeat-expr: _parent.num_fields
      vertex_field_sint16:
        params:
          - id: num_values
            type: s4
        seq:
          - id: values
            type: s2
            repeat: expr
            repeat-expr: num_values
      vertex_field_unorm8:
        params:
          - id: num_values
            type: s4
        seq:
          - id: values
            type: u1
            repeat: expr
            repeat-expr: num_values
      vertex_field_uint8:
        params:
          - id: num_values
            type: s4
        seq:
          - id: values
            type: u1
            repeat: expr
            repeat-expr: num_values
      vertex_field_unknown:
        params:
          - id: field_index
            type: s4
        instances:
          is_last_field:
            # this _root.meshes[0].fields stuff will not work since mesh[n] can have a different number of fields
            value: field_index == _parent._parent.fields.size - 1
          ofs:
            value: _parent._parent.fields[field_index].data.as<record_element>.offset
          ofs_next:
            value: _parent._parent.fields[field_index + 1].data.as<record_element>.offset
            if: is_last_field == false
          len_unk1:
            value: 'is_last_field ? _parent._io.size.as<u4> - ofs : ofs_next - ofs'
        seq:
          - id: unk1
            size: len_unk1
      faces_struct:
        seq:
          - id: unk1_type_marker
            type: u1
            enum: record_type
          - id: face
            type: face_struct
            repeat: expr
            repeat-expr: _parent.num_faces / 3
      face_struct:
        seq:
          - id: f1
            type: u2
          - id: f2
            type: u2
          - id: f3
            type: u2
  record_type_lookahead:
    seq:
      - id: save_ofs
        size: 0
        if: ofs < 0
    instances:
      ofs:
        value: _io.pos
      type1:
        pos: ofs
        type: u1
        enum: record_type
  record:
    seq:
      - id: type1
        type: u1
        enum: record_type
      - id: data
        type:
          switch-on: type1
          cases:
            #'record_type::array': record_array
            'record_type::element': record_element
            'record_type::condensed_element': record_condensed_element
            #_: record_condensed_element
    instances:
      element_value_str:
        value: 'type1 == record_type::element ? data.as<record_element>.value.str : type1 == record_type::condensed_element ? data.as<record_condensed_element>.value.str : ""'
  record_array:
    seq:
      - id: num_elements
        type: u4
      - id: elements
        type: record
        repeat: expr
        repeat-expr: num_elements
  record_element:
    seq:
      - id: key
        type: len_str
      - id: value_id
        type: u4
      - id: value
        type: len_str
      - size: 4 # unknown
      - id: offset
        type: u4
      - id: type
        type: u1
        enum: record_element_type
  record_condensed_element:
    seq:
      - id: field_index
        type: u4
      - id: value_id
        type: u4
      - id: value
        type: len_str
      - size: 4 # unknown
      - id: offset
        type: u4
      - id: type
        type: u1
        enum: record_element_type
  len_str:
    seq:
      - id: size_str
        type: u4
      - id: str
        type: str
        size: size_str
  hgm_armature:
    seq:
      - id: name
        type: len_str
      - id: num_bones
        type: u4
      - size: 6
      - size: 4
        if: _root.header.version >= 12
      - id: bones
        type: bip
        repeat: expr
        repeat-expr: num_bones
      - id: joints
        type: temp1
        size: 64
        repeat: expr
        repeat-expr: num_bones
  bip:
    seq:
      - id: name
        type: len_str
      - id: group_index
        type: u2
  temp1:
    seq:
      - id: f
        type: f4
        repeat: expr
        repeat-expr: 16
  bbox:
    seq:
      - id: values
        type: f4
        repeat: expr
        repeat-expr: 6
enums:
  vertex_type_marker:
    0: not_set
    2: set
  record_type:
    0: array
    1: condensed_element
    2: element
  record_element_type:
    0: field
    2: padding